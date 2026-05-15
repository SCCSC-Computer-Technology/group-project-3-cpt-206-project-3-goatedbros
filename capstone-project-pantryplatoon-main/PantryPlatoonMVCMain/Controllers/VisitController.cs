using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;
using PantryPlatoonMVCMain.ViewModels;

namespace PantryPlatoonMVCMain.Controllers
{
    public class VisitController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VisitController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Visit
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Visits.Include(v => v.Campus).Include(v => v.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // Added this for user specific visit
        // GET: Visit/UserVisits/5
        public async Task<IActionResult> UserVisits(string userId)
        {
            var visits = await _context.Visits
                .Include(v => v.Campus)
                .Include(v => v.User)
                .Where(v => v.UserId == userId)
                .ToListAsync();

            ViewData["UserId"] = userId;
            return View("Index", visits); // Reuse the Index view
        }


        // GET: Visit/Details/5
        // GET: Visit/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include(v => v.Campus)
                .Include(v => v.User)
                .Include(v => v.VisitItems)
                    .ThenInclude(vi => vi.Item)
                .FirstOrDefaultAsync(v => v.VisitId == id);

            if (visit == null)
            {
                return NotFound();
            }

            var viewModel = new VisitDisplayViewModel
            {
                VisitId = visit.VisitId,
                VisitDate = visit.VisitDate,
                VisitDateTime = visit.VisitDate.ToDateTime(TimeOnly.MinValue),
                ScheduledPickupTime = visit.ScheduledPickupTime,
                IsScheduledVisit = visit.IsScheduledVisit,
                UserId = visit.UserId,
                FirstName = visit.User?.FirstName ?? "",
                LastName = visit.User?.LastName ?? "",
                CampusId = visit.CampusId,
                CampusName = visit.Campus?.CampusName ?? "N/A",
                TotalWeight = visit.TotalWeight,
                Notes = visit.Notes,
                Items = visit.VisitItems.Select(vi => new VisitItemDisplayViewModel
                {
                    ItemName = vi.Item?.ItemName ?? "Unknown",
                    Quantity = vi.Quantity,
                    Weight = vi.Weight
                }).ToList()
            };

            return View(viewModel);
        }


        // GET: Visit/Create
        // GET: Visit/Create
        public async Task<IActionResult> Create(string? userId)
        {
            ViewData["CampusId"] = new SelectList(_context.Campuses, "CampusId", "CampusName");

            if (!string.IsNullOrEmpty(userId))
            {
                // Prepare a SelectList with only this user, so dropdown shows only one user
                var user = await _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => new { u.Id, u.Email })
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    ViewData["UserId"] = new SelectList(new[] { user }, "Id", "Email", user.Id);
                }
                else
                {
                    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
                }
            }
            else
            {
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            }

            return View();
        }


        // POST: Visit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // I updated this part to reflect possibility for future scheduled visits
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VisitId,UserId,CampusId,VisitDate,TotalWeight,Notes,ScheduledPickupTime,IsScheduledVisit")] Visit visit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CampusId"] = new SelectList(_context.Campuses, "CampusId", "Name", visit.CampusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", visit.UserId);
            return View(visit);
        }


        // GET: Visit/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits.FindAsync(id);
            if (visit == null)
            {
                return NotFound();
            }
            ViewData["CampusId"] = new SelectList(_context.Campuses, "CampusId", "CampusName", visit.CampusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", visit.UserId);
            return View(visit);
        }

        // POST: Visit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Updated this section as well, user can edit visits (including future visits)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VisitId,UserId,CampusId,VisitDate,TotalWeight,Notes,ScheduledPickupTime,IsScheduledVisit")] Visit visit)
        {
            if (id != visit.VisitId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Visits.Any(e => e.VisitId == visit.VisitId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CampusId"] = new SelectList(_context.Campuses, "CampusId", "CampusName", visit.CampusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", visit.UserId);
            return View(visit);
        }


        // GET: Visit/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include(v => v.Campus)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.VisitId == id);
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // POST: Visit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit != null)
            {
                _context.Visits.Remove(visit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitExists(int id)
        {
            return _context.Visits.Any(e => e.VisitId == id);
        }
    }
}
