using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PantryPlatoonMVCMain.Models
{
    public class ApplicationUser : IdentityUser
    {        
        //public string? Id { get; set; }

        public string? SCCId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [NotMapped]
        public IList<string> RoleNames { get; set; } = null!;

        public int? CampusId { get; set; }

        [ForeignKey("CampusId")]
        public Campus? Campus { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();

        // Calculated fields
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";


    }
}
