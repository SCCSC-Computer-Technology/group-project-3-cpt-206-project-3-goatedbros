namespace PantryPlatoonMVCMain.ViewModels
{
    public class UserDisplayViewModel
    {
        public string IdentityId { get; set; } = "";  // ApplicationUser.Id
        public string Email { get; set; } = "";
        public IList<string> RoleNames { get; set; } = new List<string>();

        // Custom User table fields
        public string? UserId { get; set; }
        public int? SCCId { get; set; }
        public string FirstName { get; set; } = "(not found)";
        public string LastName { get; set; } = "";

        // New property details user visit history (if any)
        public List<VisitDisplayViewModel> VisitHistory { get; set; } = new();
    }
}
