using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Inventory.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Web.Controllers
{
    public class AuthController : Controller
    {
        private const string AUTH_NAME = "GoogleAuthMiddleware";
        private const string SESSION_NAME = "GoogleAuth";
        private readonly GoogleApiConnector _api;
        private readonly InventoryContext _db;

        public AuthController(GoogleApiConnector api, InventoryContext db)
        {
            _api = api;
            _db = db;
        }

        [HttpGet]
        [Route("auth/google-login")]
        public IActionResult Login()
        {
            return Redirect(_api.LoginRequestPath());
        }

        [HttpGet]
        [Route("auth/google/callback")]
        public async Task<IActionResult> GoogleCallback(string code)
        {
            var token = await _api.TokenExchange(code);
            var googleUser = await _api.GoogleUser(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == googleUser.Email);
            if (user == null)
            {
                return RedirectToAction("Beta", "Home");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "user"),
                new Claim(ClaimTypes.Name, googleUser.Name),
                new Claim(ClaimTypes.Email, googleUser.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "GoogleAuthLogin");
            var principal = new ClaimsPrincipal(new[] { identity });
            await AuthenticationHttpContextExtensions.SignInAsync(HttpContext, AUTH_NAME, principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("auth/logout")]
        public async Task<IActionResult> Logout()
        {
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, AUTH_NAME);
            return RedirectToAction("Beta", "Home");
        }
    }
}