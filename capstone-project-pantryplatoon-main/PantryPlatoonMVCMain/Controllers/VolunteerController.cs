using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PantryPlatoonMVCMain.Controllers
{
    [Authorize]
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var shifts = await _context.VolunteerShifts.OrderByDescending(s => s.ShiftDate).ToListAsync();
            return View(shifts);
        }

        public IActionResult Create()
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VolunteerName, ShiftDate, HoursTracked, TaskDescription")] VolunteerShift volunteerShift)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }
            if (ModelState.IsValid)
            {
                _context.Add(volunteerShift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(volunteerShift);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.VolunteerShifts.FirstOrDefaultAsync(m => m.Id == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }
            var shift = await _context.VolunteerShifts.FindAsync(id);
            if (shift != null)
            {
                _context.VolunteerShifts.Remove(shift);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RequestShift()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestShift([Bind("VolunteerName,RequestedDate,HoursRequested,JobDescription")] VolunteerRequest volRequest)
        {
            if (ModelState.IsValid)
            {
                volRequest.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                volRequest.Status = "Pending";

                _context.Add(volRequest);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Your shift request has been submitted to the administration for review!";
                return RedirectToAction(nameof(Index));
            }
            return View(volRequest);
        }

        public async Task<IActionResult> ManageRequests()
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var requests = await _context.VolunteerRequests
                .Where(r => r.Status == "Pending")
                .OrderBy(r => r.RequestedDate)
                .ToListAsync();

            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var request = await _context.VolunteerRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            // FIXED TYPOS HERE: VolunteerName and UserId
            var confirmedShift = new VolunteerShift
            {
                VolunteerName = request.VolunteerName,
                ShiftDate = request.RequestedDate,
                HoursTracked = request.HoursRequested,
                TaskDescription = request.JobDescription,
                UserID = request.UserId
            };

            _context.VolunteerShifts.Add(confirmedShift);
            _context.VolunteerRequests.Remove(request);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageRequests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisapproveRequest(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var request = await _context.VolunteerRequests.FindAsync(id);
            if (request != null)
            {
                _context.VolunteerRequests.Remove(request);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ManageRequests));
        }
    }
}