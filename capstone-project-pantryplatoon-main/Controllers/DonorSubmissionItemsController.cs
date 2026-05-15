using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Capstone2025_PantryPlatoon.Data;
using Capstone2025_PantryPlatoon.Models;

namespace Capstone2025_PantryPlatoon.Controllers
{
    public class DonorSubmissionItemsController : Controller
    {
        private readonly PantryPlatoonDbContext _context;

        public DonorSubmissionItemsController(PantryPlatoonDbContext context)
        {
            _context = context;
        }

        // GET: DonorSubmissionItems
        public async Task<IActionResult> Index()
        {
            var pantryPlatoonDbContext = _context.DonorSubmissionItems!.Include(d => d.Item);
            return View(await pantryPlatoonDbContext.ToListAsync());
        }

        // GET: DonorSubmissionItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donorSubmissionItem = await _context.DonorSubmissionItems!
                .Include(d => d.Item)
                .FirstOrDefaultAsync(m => m.SubmissionItemId == id);
            if (donorSubmissionItem == null)
            {
                return NotFound();
            }

            return View(donorSubmissionItem);
        }

        // GET: DonorSubmissionItems/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemId");
            return View();
        }

        // POST: DonorSubmissionItems/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubmissionItemId,Quantity,SubmissionId,ItemId")] DonorSubmissionItem donorSubmissionItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donorSubmissionItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemId", donorSubmissionItem.ItemId);
            return View(donorSubmissionItem);
        }

        // GET: DonorSubmissionItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donorSubmissionItem = await _context.DonorSubmissionItems!.FindAsync(id);
            if (donorSubmissionItem == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemId", donorSubmissionItem.ItemId);
            return View(donorSubmissionItem);
        }

        // POST: DonorSubmissionItems/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubmissionItemId,Quantity,SubmissionId,ItemId")] DonorSubmissionItem donorSubmissionItem)
        {
            if (id != donorSubmissionItem.SubmissionItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donorSubmissionItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonorSubmissionItemExists(donorSubmissionItem.SubmissionItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemId", donorSubmissionItem.ItemId);
            return View(donorSubmissionItem);
        }

        // GET: DonorSubmissionItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donorSubmissionItem = await _context.DonorSubmissionItems!
                .Include(d => d.Item)
                .FirstOrDefaultAsync(m => m.SubmissionItemId == id);
            if (donorSubmissionItem == null)
            {
                return NotFound();
            }

            return View(donorSubmissionItem);
        }

        // POST: DonorSubmissionItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donorSubmissionItem = await _context.DonorSubmissionItems!.FindAsync(id);
            if (donorSubmissionItem != null)
            {
                _context.DonorSubmissionItems.Remove(donorSubmissionItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonorSubmissionItemExists(int id)
        {
            return _context.DonorSubmissionItems!.Any(e => e.SubmissionItemId == id);
        }
    }
}
