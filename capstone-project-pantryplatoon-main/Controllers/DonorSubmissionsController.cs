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
    public class DonorSubmissionsController : Controller
    {
        private readonly PantryPlatoonDbContext _context;

        public DonorSubmissionsController(PantryPlatoonDbContext context)
        {
            _context = context;
        }

        // GET: DonorSubmissions
        public async Task<IActionResult> Index()
        {
            var pantryPlatoonDbContext = _context.DonorSubmissions!.Include(d => d.Campus).Include(d => d.Donor);
            return View(await pantryPlatoonDbContext.ToListAsync());
        }

        // GET: DonorSubmissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donorSubmission = await _context.DonorSubmissions!
                .Include(d => d.Campus)
                .Include(d => d.Donor)
                .FirstOrDefaultAsync(m => m.SubmissionId == id);
            if (donorSubmission == null)
            {
                return NotFound();
            }

            return View(donorSubmission);
        }

        // GET: DonorSubmissions/Create
        public IActionResult Create()
        {
            ViewData["CampusId"] = new SelectList(_context.Campuses, "CampusId", "CampusId");
            ViewData["DonorId"] = new SelectList(_context.Donors, "DonorId", "DonorId");
            return View();
        }

        // POST: DonorSubmissions/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubmissionId,SubmissionDate,Notes,DonorId,CampusId")] DonorSubmission donorSubmission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donorSubmission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CampusId"] = new SelectList(_context.Campuses, "CampusId", "CampusId", donorSubmission.CampusId);
            ViewData["DonorId"] = new SelectList(_context.Donors, "DonorId", "DonorId", donorSubmission.DonorId);
            return View(donorSubmission);
        }

        // GET: DonorSubmissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donorSubmission = await _context.DonorSubmissions!.FindAsync(id);
            if (donorSubmission == null)
            {
                return NotFound();
            }
            ViewData["CampusId"] = new SelectList(_context.Campuses, "CampusId", "CampusId", donorSubmission.CampusId);
            ViewData["DonorId"] = new SelectList(_context.Donors, "DonorId", "DonorId", donorSubmission.DonorId);
            return View(donorSubmission);
        }

        // POST: DonorSubmissions/Edit/5       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubmissionId,SubmissionDate,Notes,DonorId,CampusId")] DonorSubmission donorSubmission)
        {
            if (id != donorSubmission.SubmissionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donorSubmission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonorSubmissionExists(donorSubmission.SubmissionId))
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
            ViewData["CampusId"] = new SelectList(_context.Campuses, "CampusId", "CampusId", donorSubmission.CampusId);
            ViewData["DonorId"] = new SelectList(_context.Donors, "DonorId", "DonorId", donorSubmission.DonorId);
            return View(donorSubmission);
        }

        // GET: DonorSubmissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donorSubmission = await _context.DonorSubmissions!
                .Include(d => d.Campus)
                .Include(d => d.Donor)
                .FirstOrDefaultAsync(m => m.SubmissionId == id);
            if (donorSubmission == null)
            {
                return NotFound();
            }

            return View(donorSubmission);
        }

        // POST: DonorSubmissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donorSubmission = await _context.DonorSubmissions!.FindAsync(id);
            if (donorSubmission != null)
            {
                _context.DonorSubmissions.Remove(donorSubmission);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonorSubmissionExists(int id)
        {
            return _context.DonorSubmissions!.Any(e => e.SubmissionId == id);
        }
    }
}
