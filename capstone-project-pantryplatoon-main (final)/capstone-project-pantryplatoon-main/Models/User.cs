// Capstone 2025 Project
// Pantry Platoon
// Team Members: Michael Lee, Colin Gaffney
// Jared Daniels, Aaron Rash, Donn Gerald

namespace Capstone2025_PantryPlatoon.Models
{
    public class User
    {
        public int UserId { get; set; }       // primary key
        public string? SCCId { get; set; }   // this is for the student/staff school ID
        public string? FirstName { get; set; }  // first name of user
        public string? LastName { get; set; }   // last name of user
        public string? Email { get; set; }
        public string? Role {  get; set; }  // Student, Admin, or Donor

        public int CampusId { get; set; }   // foreign key linking Campus table
        public Campus? Campus { get; set; }
        public ICollection<Visit>? Visits { get; set; } // one-to-many relationship between User and Visit tables
    }
}
