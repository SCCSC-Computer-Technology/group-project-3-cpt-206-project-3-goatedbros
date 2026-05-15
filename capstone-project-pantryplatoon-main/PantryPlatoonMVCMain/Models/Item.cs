using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PantryPlatoonMVCMain.Models;

[Table("Item")]
public partial class Item
{
    [Key]
    public int ItemId { get; set; }

    [StringLength(100)]
    public string ItemName { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    public int? ItemCategoryId { get; set; }

    public int? Points { get; set; }

    public int? Quantity { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Weight { get; set; }
    [InverseProperty("Item")]
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [ForeignKey("ItemCategoryId")]
    [InverseProperty("Items")]
    public virtual ItemCategory? ItemCategory { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<VisitItem> VisitItems { get; set; } = new List<VisitItem>();

    [StringLength(250)]
    public string? ImagePath { get; set; }

    [NotMapped]
    public string ImageUrl => !string.IsNullOrEmpty(ImagePath) ? $"/images/items/{ImagePath}" : "/images/items/default-item.png";

}