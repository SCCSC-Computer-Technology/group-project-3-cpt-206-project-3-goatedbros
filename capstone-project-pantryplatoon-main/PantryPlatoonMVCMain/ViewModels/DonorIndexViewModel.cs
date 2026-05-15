using PantryPlatoonMVCMain.Models;

namespace PantryPlatoonMVCMain.ViewModels
{
    public class DonorIndexViewModel
    {
        public List<Donation> Donations { get; set; } = new List<Donation>();
        public List<Campus> Campuses { get; set; } = new List<Campus>();
        public int? CampusId { get; set; }
    }
}