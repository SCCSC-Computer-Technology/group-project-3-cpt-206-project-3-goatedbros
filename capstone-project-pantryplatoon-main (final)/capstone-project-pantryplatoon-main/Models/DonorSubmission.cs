// Capstone 2025 Project
// Pantry Platoon
// Team Members: Michael Lee, Colin Gaffney
// Jared Daniels, Aaron Rash, Donn Gerald

namespace Capstone2025_PantryPlatoon.Models
{
    public class DonorSubmission
    {
        public int SubmissionId { get; set; }           // primary key
        public DateTime SubmissionDate { get; set; }    // date donor submitted item(s)
        public string? Notes { get; set; }              // descriptive donor notes 
        public int DonorId { get; set; }                // foreign key linking Donor and DonorSubmission tables
        public Donor? Donor { get; set; }               
        public int CampusId { get; set; }               // foreign key linking DonorSubmission and Campus tables
        public Campus? Campus { get; set; }
    }
}
