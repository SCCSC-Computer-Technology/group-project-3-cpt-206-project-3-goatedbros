// Capstone 2025 Project
// Pantry Platoon
// Team Members: Michael Lee, Colin Gaffney
// Jared Daniels, Aaron Rash, Donn Gerald

namespace Capstone2025_PantryPlatoon.Models
{
    // model
    public class Visit
    {
        public int VisitId { get; set; }            // primary key
        public DateTime VisitDate { get; set; }     // date and time of each visit
        public string? Notes { get; set; }          // staff (admin) can leave comments about each visit
        public int TotalWeight { get; set; }
        public int UserId { get; set; }             // foreign key linking User table
        public User? User { get; set; }
        public int CampusId { get; set; }           // foreign key linking Campus table
        public Campus? Campus { get; set; }

        public ICollection<VisitItem>? VisitItems { get; set; } // one-to-many relationship between VisitItem and Visit tables
    }
}
