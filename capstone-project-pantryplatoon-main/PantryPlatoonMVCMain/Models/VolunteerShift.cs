using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PantryPlatoonMVCMain.Models
{
    public class VolunteerShift
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Volunteer Name")]
        public string VolunteerName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Shift Date")]
        [DataType(DataType.Date)]
        public DateTime ShiftDate { get; set; }

        [Required]
        [Display(Name = "Hours Worked")]
        [Range(0.5, 24, ErrorMessage = "Hours worked must be between 0.5 and 24")]
        public double HoursTracked { get; set; }

        [Required]
        [Display(Name = "Task Description")]
        public string TaskDescription { get; set; } = string.Empty;

        public string? UserID { get; set; }
    }
}
