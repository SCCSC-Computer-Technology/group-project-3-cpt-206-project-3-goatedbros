using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PantryPlatoonMVCMain.Models;

[Table("Campus")]
public partial class Campus
{
    [Key]
    public int CampusId { get; set; }

    [StringLength(100)]
    public string CampusName { get; set; } = null!;

    [StringLength(255)]
    public string? Address { get; set; }

    [InverseProperty("Campus")]
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [InverseProperty("Campus")]
    public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

    [InverseProperty("Campus")]
    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
