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
    public class RoomsController : Controller
    {
        private readonly InventoryContext _context;

        public RoomsController(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Rooms.ToListAsync());
        }

        [HttpGet]
        [Route("/rooms/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        [HttpGet]
        [Route("/rooms/new")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/rooms/new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Room room)
        {
            if (ModelState.IsValid)
            {
                if (UserId() == null)
                {
                    return Unauthorized();
                }
                room.UserId = (int)UserId();
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        [HttpGet]
        [Route("/rooms/{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        [HttpPost]
        [Route("/rooms/{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description")] Room room)
        {
            if (!_context.Rooms.Any(e => e.Id == id))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (UserId() == null)
                {
                    return Unauthorized();
                }
                room.UserId = (int)UserId();
                room.Id = id;
                _context.Update(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        [HttpPost]
        [Route("/rooms/{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);
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
