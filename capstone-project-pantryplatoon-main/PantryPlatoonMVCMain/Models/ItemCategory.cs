using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PantryPlatoonMVCMain.Models;

[Table("ItemCategory")]
public partial class ItemCategory
{
    [Key]
    public int ItemCategoryId { get; set; }

    [StringLength(100)]
    public string ItemCategoryName { get; set; } = null!;

    [InverseProperty("ItemCategory")]
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
