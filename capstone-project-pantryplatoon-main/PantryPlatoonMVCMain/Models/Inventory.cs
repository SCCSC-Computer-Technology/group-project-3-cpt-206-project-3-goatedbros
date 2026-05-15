using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PantryPlatoonMVCMain.Models;

[Table("Inventory")]
public partial class Inventory
{
    [Key]
    public int InventoryId { get; set; }

    public int? ItemId { get; set; }

    public int? CampusId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastUpdated { get; set; }

    [ForeignKey("CampusId")]
    [InverseProperty("Inventories")]
    public virtual Campus? Campus { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("Inventories")]
    public virtual Item? Item { get; set; }
}
