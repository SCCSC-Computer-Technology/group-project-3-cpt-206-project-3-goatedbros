// Capstone 2025 Project
// Pantry Platoon
// Team Members: Michael Lee, Colin Gaffney
// Jared Daniels, Aaron Rash, Donn Gerald

using System.Linq.Expressions;

namespace Capstone2025_PantryPlatoon.Models
{
    // model
    public class Campus
    {
        public int CampusId {  get; set; }      // primary key
        public string? CampusName { get; set; } // name of campus
        public string? Address { get; set; }    // campus address

        public ICollection<User>? Users { get; set; }   // one-to-many between User and Campus tables
        public ICollection<Visit>? Visits { get; set; } // one-to-many between Visit and Campus tables
    }
}
