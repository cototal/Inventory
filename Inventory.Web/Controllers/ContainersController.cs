using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Web.Models;
using Inventory.Web.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;

namespace Inventory.Web.Controllers
{
    [Authorize]
    public class ContainersController : Controller
    {
        private readonly InventoryContext _context;

        public ContainersController(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Containers.ToListAsync());
        }

        [HttpGet]
        [Route("/containers/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var container = await _context.Containers.FirstOrDefaultAsync(m => m.Id == id);
            if (container == null)
            {
                return NotFound();
            }

            return View(container);
        }

        [HttpGet]
        [Route("/containers/new")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/containers/new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Container container)
        {
            if (ModelState.IsValid)
            {
                if (UserId() == null)
                {
                    return Unauthorized();
                }
                container.UserId = (int)UserId();
                _context.Add(container);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(container);
        }

        [HttpGet]
        [Route("/containers/{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var container = await _context.Containers.FirstOrDefaultAsync(m => m.Id == id);
            if (container == null)
            {
                return NotFound();
            }
            return View(container);
        }

        [HttpPost]
        [Route("/containers/{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description")] Container container)
        {
            if (!_context.Containers.Any(e => e.Id == id))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (UserId() == null)
                {
                    return Unauthorized();
                }
                container.UserId = (int)UserId();
                container.Id = id;
                _context.Update(container);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(container);
        }

        [HttpPost]
        [Route("/containers/{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var container = await _context.Containers.FindAsync(id);
            _context.Containers.Remove(container);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private int? UserId()
        {
            var idClaim = User.Claims.Where(c => c.Type == ClaimTypes.Sid).FirstOrDefault();
            if (idClaim == null)
            {
                return null;
            }
            return Convert.ToInt32(idClaim.Value);
        }
    }
}
