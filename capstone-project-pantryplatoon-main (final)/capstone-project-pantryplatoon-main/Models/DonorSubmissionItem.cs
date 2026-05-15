// Capstone 2025 Project
// Pantry Platoon
// Team Members: Michael Lee, Colin Gaffney
// Jared Daniels, Aaron Rash, Donn Gerald

namespace Capstone2025_PantryPlatoon.Models
{
    public class DonorSubmissionItem
    {
        public int SubmissionItemId { get; set; }       // primary key
        public int Quantity { get; set; }               // how many quantities donated
        public int SubmissionId { get; set; }           // foreign key linking DonorSubmissionItem and DonorSubmission tables
        public DonorSubmission? DonorSubmission { get; set; }
        public int ItemId { get; set; }                 // foreign key linking DonorSubmissionItem and Item tables
        public Item? Item { get; set; }
    }
}
