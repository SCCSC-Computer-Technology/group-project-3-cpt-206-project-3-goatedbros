using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Capstone2025_PantryPlatoon.Data;
using Capstone2025_PantryPlatoon.Models;

namespace Capstone2025_PantryPlatoon.Controllers
{
    public class InventoriesController : Controller
    {
        private readonly PantryPlatoonDbContext _context;

        public InventoriesController(PantryPlatoonDbContext context)
        {
            _context = context;
        }

        // GET: Inventories
        public async Task<IActionResult> Index()
        {
            if (_context.Inventory == null)
                return Problem("Inventory entity set is null.");

            var inventories = _context.Inventory
                .Include(i => i.Campus)
                .Include(i => i.Item);

            return View(await inventories.ToListAsync());
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inventory == null)
                return NotFound();

            var inventory = await _context.Inventory
                .Include(i => i.Campus)
                .Include(i => i.Item)
                .FirstOrDefaultAsync(m => m.InventoryId == id);

            if (inventory == null)
                return NotFound();

            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Inventories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InventoryId,Quantity,LastUpdated,ItemId,CampusId")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                inventory.LastUpdated = DateTime.UtcNow;
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(inventory.CampusId, inventory.ItemId);
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inventory == null)
                return NotFound();

            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null)
                return NotFound();

            PopulateDropdowns(inventory.CampusId, inventory.ItemId);
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InventoryId,Quantity,LastUpdated,ItemId,CampusId")] Inventory inventory)
        {
            if (id != inventory.InventoryId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    inventory.LastUpdated = DateTime.UtcNow;
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.InventoryId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(inventory.CampusId, inventory.ItemId);
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inventory == null)
                return NotFound();

            var inventory = await _context.Inventory
                .Include(i => i.Campus)
                .Include(i => i.Item)
                .FirstOrDefaultAsync(m => m.InventoryId == id);

            if (inventory == null)
                return NotFound();

            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inventory == null)
                return Problem("Inventory entity set is null.");

            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventory.Remove(inventory);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventory?.Any(e => e.InventoryId == id) ?? false;
        }

        // Helper method to populate dropdowns
        private void PopulateDropdowns(int? selectedCampusId = null, int? selectedItemId = null)
        {
            ViewData["CampusId"] = new SelectList(_context.Campuses, "CampusId", "CampusName", selectedCampusId);
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemName", selectedItemId);
        }
    }
}
