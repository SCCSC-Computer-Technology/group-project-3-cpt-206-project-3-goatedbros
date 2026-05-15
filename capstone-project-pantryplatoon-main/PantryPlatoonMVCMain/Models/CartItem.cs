using System.ComponentModel.DataAnnotations.Schema;

namespace PantryPlatoonMVCMain.Models
{
    public class CartItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public int Points { get; set; }
        public int Quantity { get; set; }

        public decimal TotalWeight => Weight * Quantity;
        public int TotalPoints => Points * Quantity;

        public string? ImagePath { get; set; }

        // Calculated fields
        [NotMapped]
        public string ImageUrl => !string.IsNullOrEmpty(ImagePath) ? $"/images/items/{ImagePath}" : "/images/items/default-item.png";

    }
}