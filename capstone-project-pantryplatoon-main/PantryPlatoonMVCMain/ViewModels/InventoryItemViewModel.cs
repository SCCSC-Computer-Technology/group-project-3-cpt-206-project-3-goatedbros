using System.ComponentModel.DataAnnotations;

namespace PantryPlatoonMVCMain.ViewModels
{
    // For the student-facing inventory page
    public class InventoryItemViewModel
    {
        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int AvailableQuantity { get; set; }
        public decimal? Weight { get; set; }
        public int? Points { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string? ImagePath { get; set; }

        // Calculated fields
        public string ImageUrl => !string.IsNullOrEmpty(ImagePath) ? $"/images/items/{ImagePath}" : "/images/items/default-item.png";

    }
}