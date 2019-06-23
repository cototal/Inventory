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
    public class ItemsController : Controller
    {
        private readonly InventoryContext _context;

        public ItemsController(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Items.ToListAsync());
        }

        [HttpGet]
        [Route("/items/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpGet]
        [Route("/items/new")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/items/new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Item item)
        {
            if (ModelState.IsValid)
            {
                if (UserId() == null)
                {
                    return Unauthorized();
                }
                item.UserId = (int)UserId();
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        [HttpGet]
        [Route("/items/{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [Route("/items/{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description")] Item item)
        {
            if (!_context.Items.Any(e => e.Id == id))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (UserId() == null)
                {
                    return Unauthorized();
                }
                item.UserId = (int)UserId();
                item.Id = id;
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        [HttpPost]
        [Route("/items/{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
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
