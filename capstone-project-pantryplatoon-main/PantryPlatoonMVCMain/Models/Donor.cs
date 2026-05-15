using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PantryPlatoonMVCMain.Models;

[Table("Donor")]
public partial class Donor
{
    [Key]
    public int DonorId { get; set; }

    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[A-Za-z\s'-]+$", ErrorMessage = "Only letters, spaces, apostrophes, and hyphens are allowed.")]
    public string DonorName { get; set; } = null!;

    [Required]
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; } = null!;

    [Required]
    [Phone]
    [RegularExpression(@"^\(\d{3}\) \d{3} - \d{4}$", ErrorMessage = "Phone number must be in the format (123) 456 - 7890")]
    public string PhoneNumber { get; set; } = null!;
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
