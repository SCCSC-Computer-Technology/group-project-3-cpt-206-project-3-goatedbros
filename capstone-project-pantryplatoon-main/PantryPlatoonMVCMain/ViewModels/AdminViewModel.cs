using Microsoft.AspNetCore.Identity;
using PantryPlatoonMVCMain.Models;

namespace PantryPlatoonMVCMain.ViewModels
{
    public class AdminViewModel
    {
        public IEnumerable<Item> Items { get; set; } = null!;
        public UserViewModel Users { get; set; } = null!;
        public IEnumerable<Visit> Visits { get; set; } = null!;
        public int DonorCount { get; set; }
    }
}
