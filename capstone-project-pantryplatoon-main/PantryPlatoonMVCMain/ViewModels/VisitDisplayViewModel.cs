namespace PantryPlatoonMVCMain.ViewModels
{
    public class VisitDisplayViewModel
    {
        public int VisitId { get; set; }
        public DateOnly VisitDate { get; set; }

        // Campus
        public int? CampusId { get; set; }
        public string CampusName { get; set; } = "N/A";

        // User
        public string? UserId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public string UserFullName => $"{FirstName} {LastName}".Trim();

        public decimal? TotalWeight { get; set; }
        public string? Notes { get; set; }

        public DateTime VisitDateTime { get; set; }

        public DateTime? ScheduledPickupTime { get; set; }
        public bool IsScheduledVisit { get; set; }

        public List<VisitItemDisplayViewModel> Items { get; set; } = new();
    }
}
