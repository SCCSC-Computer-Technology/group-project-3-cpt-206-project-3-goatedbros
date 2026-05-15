using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PantryPlatoonMVCMain.Models;

[Table("Visit")]
public partial class Visit
{
    [Key]
    public int VisitId { get; set; }

    public string? UserId { get; set; }

    public int? CampusId { get; set; }

    public DateOnly VisitDate { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? TotalWeight { get; set; }

    public string? Notes { get; set; }

    public DateTime? ScheduledPickupTime { get; set; }

    public bool IsScheduledVisit { get; set; } = false;

    [ForeignKey("CampusId")]
    [InverseProperty("Visits")]
    public virtual Campus? Campus { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Visits")]
    public virtual ApplicationUser? User { get; set; }

    [InverseProperty("Visit")]
    public virtual ICollection<VisitItem> VisitItems { get; set; } = new List<VisitItem>();
}
