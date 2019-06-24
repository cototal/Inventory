using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Web.Models;
using Inventory.Web.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var items = await _context.Items.Include(i => i.Container).ThenInclude(c => c.Room).ToListAsync();
            return View(items);
        }

        [HttpGet]
        [Route("/items/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Items
                .Include(i => i.Container).ThenInclude(c => c.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpGet]
        [Route("/items/new")]
        public async Task<IActionResult> Create()
        {
            var containers = (await _context.Containers.ToListAsync()).Select(co => new SelectListItem
            {
                Text = co.Name,
                Value = co.Id.ToString(),
                Selected = false
            }).ToList();
            ViewData["containers"] = containers;
            return View();
        }

        [HttpPost]
        [Route("/items/new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContainerId,Name,Description")] Item item)
        {
            if (ModelState.IsValid)
            {
                var container = await GetContainer(item.ContainerId);
                if (container == null)
                {
                    return NotFound();
                }
                item.Container = container;
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
            var containers = (await _context.Containers.ToListAsync()).Select(co => new SelectListItem
            {
                Text = co.Name,
                Value = co.Id.ToString(),
                Selected = false
            }).ToList();
            ViewData["containers"] = containers;
            var item = await _context.Items
                .Include(i => i.Container).ThenInclude(c => c.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [Route("/items/{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContainerId,Name,Description")] Item item)
        {
            if (!_context.Items.Any(e => e.Id == id))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var container = await GetContainer(item.ContainerId);
                if (container == null)
                {
                    return NotFound();
                }
                item.Container = container;
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
        private async Task<Container> GetContainer(int containerId)
        {
            var container = await _context.Containers
                .Include(r => r.Room)
                .FirstOrDefaultAsync(c => c.Id == containerId);

            if (container == null)
            {
                return null;
            }

            if (container.Room.UserId != UserId())
            {
                return null;
            }
            return container;
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
