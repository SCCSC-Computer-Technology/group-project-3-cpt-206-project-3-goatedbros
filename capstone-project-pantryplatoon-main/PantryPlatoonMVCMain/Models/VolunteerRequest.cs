using System;
using System.ComponentModel.DataAnnotations;

namespace PantryPlatoonMVCMain.Models
{
    public class VolunteerRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Your Name")]
        public string VolunteerName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Requested Date")]
        [DataType(DataType.Date)]
        public DateTime RequestedDate { get; set; }

        [Required]
        [Display(Name = "Hours")]
        [Range(0.5, 24, ErrorMessage = "Hours must be between 0.5 and 24")]
        public double HoursRequested { get; set; }

        [Required]
        [Display(Name = "Preferred Task")]
        public string JobDescription { get; set; } = string.Empty;

        public string? UserId { get; set; }
        public string Status { get; set; } = "Pending";

    }
}
