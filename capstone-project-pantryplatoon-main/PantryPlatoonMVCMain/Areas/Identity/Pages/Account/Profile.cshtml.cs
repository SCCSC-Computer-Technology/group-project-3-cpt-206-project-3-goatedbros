using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;

namespace PantryPlatoonMVCMain.Areas.Identity.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Properties for the view
        public ApplicationUser CurrentUser { get; set; } = null!;
        public List<Visit> RecentVisits { get; set; } = new List<Visit>();
        public int TotalVisits { get; set; }
        public int TotalItemsReceived { get; set; }
        public decimal TotalWeight { get; set; }
        public string? StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Load user with campus information
            CurrentUser = await _userManager.Users
                .Include(u => u.Campus)
                .FirstOrDefaultAsync(u => u.Id == user.Id) ?? user;

            // Get user's visits - assuming Visit table now links to ApplicationUser via SCCId or UserId
            var allVisits = await _context.Visits
                .Include(v => v.Campus)
                .Include(v => v.VisitItems)
                    .ThenInclude(vi => vi.Item)
                        .ThenInclude(i => i.ItemCategory)
                .Where(v => v.User != null && v.User.SCCId == CurrentUser.SCCId)
                .OrderByDescending(v => v.VisitDate)
                .ToListAsync();

            RecentVisits = allVisits.Take(10).ToList();
            TotalVisits = allVisits.Count;
            TotalItemsReceived = allVisits.SelectMany(v => v.VisitItems).Sum(vi => vi.QuantityTaken);
            TotalWeight = allVisits.Sum(v => v.TotalWeight ?? 0);

            StatusMessage = TempData["StatusMessage"] as string;

            return Page();
        }

        public async Task<IActionResult> OnGetVisitDetailsAsync(int visitId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include(v => v.Campus)
                .Include(v => v.User)
                .Include(v => v.VisitItems)
                    .ThenInclude(vi => vi.Item)
                        .ThenInclude(i => i.ItemCategory)
                .FirstOrDefaultAsync(v => v.VisitId == visitId && v.User != null && v.User.SCCId == currentUser.SCCId);

            if (visit == null)
            {
                TempData["StatusMessage"] = "Error: Visit not found or you don't have permission to view it.";
                return RedirectToPage();
            }

            // Return JSON for AJAX request or redirect for regular request
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var visitData = new
                {
                    visitId = visit.VisitId,
                    visitDate = visit.VisitDate.ToString("MMMM dd, yyyy"),
                    dayOfWeek = visit.VisitDate.ToString("dddd"),
                    campus = visit.Campus?.CampusName,
                    campusAddress = visit.Campus?.Address,
                    totalWeight = visit.TotalWeight?.ToString("F1"),
                    notes = visit.Notes,
                    items = visit.VisitItems.Select(vi => new
                    {
                        name = vi.Item?.ItemName,
                        quantity = vi.QuantityTaken,
                        category = vi.Item?.ItemCategory?.ItemCategoryName,
                        description = vi.Item?.Description,
                        weight = vi.Item?.Weight
                    }).ToList()
                };

                return new JsonResult(visitData);
            }

            // For non-AJAX requests, redirect back with visit data in TempData
            TempData["VisitDetails"] = System.Text.Json.JsonSerializer.Serialize(visit);
            return RedirectToPage();
        }
    }
}