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
    public class AdminReportsController : Controller
    {
        private readonly PantryPlatoonDbContext _context;

        public AdminReportsController(PantryPlatoonDbContext context)
        {
            _context = context;
        }

        // GET: AdminReports
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdminReports!.ToListAsync());
        }

        // GET: AdminReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminReports = await _context.AdminReports!
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (adminReports == null)
            {
                return NotFound();
            }

            return View(adminReports);
        }

        // GET: AdminReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminReports/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportId,Month,TotalVisits,TotalStudentsServed,RepeatVisitors,TotalItems,TotalWeight")] AdminReports adminReports)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminReports);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminReports);
        }

        // GET: AdminReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminReports = await _context.AdminReports!.FindAsync(id);
            if (adminReports == null)
            {
                return NotFound();
            }
            return View(adminReports);
        }

        // POST: AdminReports/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportId,Month,TotalVisits,TotalStudentsServed,RepeatVisitors,TotalItems,TotalWeight")] AdminReports adminReports)
        {
            if (id != adminReports.ReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminReports);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminReportsExists(adminReports.ReportId))
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
            return View(adminReports);
        }

        // GET: AdminReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminReports = await _context.AdminReports!
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (adminReports == null)
            {
                return NotFound();
            }

            return View(adminReports);
        }

        // POST: AdminReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminReports = await _context.AdminReports!.FindAsync(id);
            if (adminReports != null)
            {
                _context.AdminReports.Remove(adminReports);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminReportsExists(int id)
        {
            return _context.AdminReports!.Any(e => e.ReportId == id);
        }
    }
}
