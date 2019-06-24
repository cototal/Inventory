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
    public class ContainersController : Controller
    {
        private readonly InventoryContext _context;

        public ContainersController(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Containers.Include(c => c.Room).ToListAsync());
        }

        [HttpGet]
        [Route("/containers/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var container = await _context.Containers.Include(c => c.Room).FirstOrDefaultAsync(m => m.Id == id);
            if (container == null)
            {
                return NotFound();
            }

            return View(container);
        }

        [HttpGet]
        [Route("/containers/new")]
        public async Task<IActionResult> Create()
        {
            var rooms = (await _context.Rooms.ToListAsync()).Select(rm => new SelectListItem
            {
                Text = rm.Name,
                Value = rm.Id.ToString(),
                Selected = false
            }).ToList();
            ViewData["rooms"] = rooms;
            return View();
        }

        [HttpPost]
        [Route("/containers/new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,Name,Description")] Container container)
        {
            if (ModelState.IsValid)
            {
                var room = await GetRoom(container.RoomId);
                if (room == null)
                {
                    return NotFound();
                }
                container.Room = room;
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
            var rooms = (await _context.Rooms.ToListAsync()).Select(rm => new SelectListItem
            {
                Text = rm.Name,
                Value = rm.Id.ToString(),
                Selected = false
            }).ToList();
            ViewData["rooms"] = rooms;
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
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,Name,Description")] Container container)
        {
            if (!_context.Containers.Any(e => e.Id == id))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var room = await GetRoom(container.RoomId);
                if (room == null)
                {
                    return NotFound();
                }
                container.Room = room;
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

        private async Task<Room> GetRoom(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                return null;
            }
            if (room.UserId != UserId())
            {
                return null;
            }
            return room;
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
