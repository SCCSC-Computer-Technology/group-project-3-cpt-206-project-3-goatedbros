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

        public int? Age { get; set; }

        public int? AdultsInHousehold { get; set; }

        public int? ChildrenUnder5 { get; set; }

        public int? Children5To18 { get; set; }

        public string? StudentStatus { get; set; }

        public string? EmploymentStatus { get; set; }

        public string? HouseholdEmploymentStatus { get; set; }

        public string? BenefitsStatus { get; set; }

        public string? KitchenAccess { get; set; }

        public bool HasDietaryRestrictions { get; set; }

        public string? DietaryRestrictionExplanation { get; set; }

        public string? SpecialtyItemsNeeded { get; set; }

        public string? SpecialtyItemsExplanation { get; set; }

        public string? AdditionalNotes { get; set; }


    }
}
