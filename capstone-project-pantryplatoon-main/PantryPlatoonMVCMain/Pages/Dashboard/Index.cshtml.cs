using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Data;
namespace PantryPlatoonMVCMain.Pages.Dashboard
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public int TotalUsers { get; set; }
        public double AverageAge { get; set; }
        public int UsersWithDietaryRestrictions { get; set; }
        public int UsersNeedingSpecialtyItems { get; set; }
        public int TotalAdultsInHouseholds { get; set; }
        public int TotalChildrenUnder5 { get; set; }
        public int TotalChildren5To18 { get; set; }
        public List<DashboardCountItem> CampusBreakdown { get; set; } = new();
        public List<DashboardCountItem> AgeBreakdown { get; set; } = new();
        public List<DashboardCountItem> StudentStatusBreakdown { get; set; } = new();
        public List<DashboardCountItem> EmploymentStatusBreakdown { get; set; } = new();
        public async Task OnGetAsync()
        {
            var users = await _context.Users
                .Include(u => u.Campus)
                .AsNoTracking()
                .ToListAsync();
            TotalUsers = users.Count;
            var usersWithAges = users
                .Where(u => u.Age.HasValue)
                .ToList();
            AverageAge = usersWithAges.Any()
                ? Math.Round(usersWithAges.Average(u => u.Age!.Value), 1)
                : 0;
            UsersWithDietaryRestrictions = users.Count(u => u.HasDietaryRestrictions);
            UsersNeedingSpecialtyItems = users.Count(u =>
                !string.IsNullOrWhiteSpace(u.SpecialtyItemsNeeded));
            TotalAdultsInHouseholds = users.Sum(u => u.AdultsInHousehold ?? 0);
            TotalChildrenUnder5 = users.Sum(u => u.ChildrenUnder5 ?? 0);
            TotalChildren5To18 = users.Sum(u => u.Children5To18 ?? 0);
            CampusBreakdown = users
                .GroupBy(u => u.Campus != null
                    ? u.Campus.CampusName
                    : "No Campus Listed")
                .Select(g => new DashboardCountItem
                {
                    Label = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();
            StudentStatusBreakdown = users
                .GroupBy(u => string.IsNullOrWhiteSpace(u.StudentStatus)
                    ? "Not Provided"
                    : u.StudentStatus)
                .Select(g => new DashboardCountItem
                {
                    Label = g.Key!,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();
            EmploymentStatusBreakdown = users
                .GroupBy(u => string.IsNullOrWhiteSpace(u.EmploymentStatus)
                    ? "Not Provided"
                    : u.EmploymentStatus)
                .Select(g => new DashboardCountItem
                {
                    Label = g.Key!,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();
            AgeBreakdown = new List<DashboardCountItem>
           {
               new()
               {
                   Label = "Under 18",
                   Count = users.Count(u => u.Age.HasValue && u.Age < 18)
               },
               new()
               {
                   Label = "18–24",
                   Count = users.Count(u => u.Age.HasValue && u.Age >= 18 && u.Age <= 24)
               },
               new()
               {
                   Label = "25–34",
                   Count = users.Count(u => u.Age.HasValue && u.Age >= 25 && u.Age <= 34)
               },
               new()
               {
                   Label = "35–44",
                   Count = users.Count(u => u.Age.HasValue && u.Age >= 35 && u.Age <= 44)
               },
               new()
               {
                   Label = "45–54",
                   Count = users.Count(u => u.Age.HasValue && u.Age >= 45 && u.Age <= 54)
               },
               new()
               {
                   Label = "55+",
                   Count = users.Count(u => u.Age.HasValue && u.Age >= 55)
               },
               new()
               {
                   Label = "Not Provided",
                   Count = users.Count(u => !u.Age.HasValue)
               }
           };
        }
    }
    public class DashboardCountItem
    {
        public string Label { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}