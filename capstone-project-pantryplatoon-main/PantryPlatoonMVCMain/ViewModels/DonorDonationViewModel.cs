using PantryPlatoonMVCMain.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PantryPlatoonMVCMain.ViewModels
{
    public class DonorDonationViewModel
    {
        public Donor Donor { get; set; } = new Donor();
        public List<Donation> Donations { get; set; } = new List<Donation>();

        [Required(ErrorMessage = "Please select a campus.")]
        public int? CampusId { get; set; }

        [BindNever]
        [ValidateNever]
        public List<Campus> Campuses { get; set; }

        public List<ItemCategory> Categories { get; set; } = new();
        public List<Item> Items { get; set; } = new();
        public List<ItemEntry> ItemEntries { get; set; } = new();
    }
    public class ItemEntry
    {
        public string ItemName { get; set; }
        public int ItemCategoryId { get; set; }
    }
}
