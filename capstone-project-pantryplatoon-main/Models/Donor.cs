// Capstone 2025 Project
// Pantry Platoon
// Team Members: Michael Lee, Colin Gaffney
// Jared Daniels, Aaron Rash, Donn Gerald

namespace Capstone2025_PantryPlatoon.Models
{
    public class Donor
    {
        public int DonorId { get; set; }        // primary key
        public string? DonorName { get; set; }  // name of donor
        public string? Email { get; set; }      // donor email address
    }
}
