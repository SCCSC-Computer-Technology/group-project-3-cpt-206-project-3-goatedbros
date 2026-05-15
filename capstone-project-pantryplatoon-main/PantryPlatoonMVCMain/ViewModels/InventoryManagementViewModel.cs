namespace PantryPlatoonMVCMain.ViewModels
{
    // For the inventory management page
    public class InventoryManagementViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int CurrentQuantity { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string? ImagePath { get; set; }

        // Calculated fields
        public string ImageUrl => !string.IsNullOrEmpty(ImagePath) ? $"/images/items/{ImagePath}" : "/images/items/default-item.png";
    }
}