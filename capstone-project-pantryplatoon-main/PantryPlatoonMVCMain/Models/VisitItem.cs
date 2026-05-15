using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PantryPlatoonMVCMain.Models;

[Table("VisitItem")]
public partial class VisitItem
{
    [Key]
    public int VisitItemId { get; set; }

    public int? VisitId { get; set; }

    public int? ItemId { get; set; }

    public int QuantityTaken { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("VisitItems")]
    public virtual Item? Item { get; set; }

    [ForeignKey("VisitId")]
    [InverseProperty("VisitItems")]
    public virtual Visit? Visit { get; set; }

    public int Quantity { get; set; }
    public double Weight { get; set; }
}
