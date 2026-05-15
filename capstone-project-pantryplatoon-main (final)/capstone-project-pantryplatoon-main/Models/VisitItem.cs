// Capstone 2025 Project
// Pantry Platoon
// Team Members: Michael Lee, Colin Gaffney
// Jared Daniels, Aaron Rash, Donn Gerald

namespace Capstone2025_PantryPlatoon.Models
{
    public class VisitItem
    {
        public int VisitItemId { get; set; }
        public int VisitId { get; set; }        // foreign key linking Visit table
        public Visit? Visit { get; set; }        
        public int ItemId { get; set; }        // foreign key linking Item table
        public Item? Item { get; set; }
        public int QuantityTaken { get; set; } // quanitities taken
    }
}
