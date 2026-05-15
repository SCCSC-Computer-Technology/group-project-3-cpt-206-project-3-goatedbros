using System.ComponentModel.DataAnnotations;

namespace PantryPlatoonMVCMain.ViewModels
{
    // For adding new items
    public class NewItemViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int ItemCategoryId { get; set; }

        [Range(0, 999.99)]
        [Display(Name = "Weight (lbs)")]
        public decimal? Weight { get; set; }

        [Range(0, 9999)]
        [Display(Name = "Points")]
        public int? Points { get; set; }

        [Range(0, 9999)]
        [Display(Name = "Initial Quantity")]
        public int InitialQuantity { get; set; } = 0;

        [Display(Name = "Item Image")]
        public IFormFile? ImageFile { get; set; }
    }
}