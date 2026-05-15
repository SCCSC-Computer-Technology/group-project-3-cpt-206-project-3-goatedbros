// Capstone 2025 Project
// Pantry Platoon
// Team Members: Michael Lee, Colin Gaffney
// Jared Daniels, Aaron Rash, Donn Gerald

namespace Capstone2025_PantryPlatoon.Models
{
    // model
    public class Item
    {
        public int ItemId { get; set; }                             // primary key
        public string? ItemName { get; set; }                       // name of item
        public string? ItemDescription { get; set; }
        public int Quantity { get; set; }                           // quantity of item
        public decimal Weight { get; set; }                         // weight of item
        public int Points { get; set; }                             // point system used to rank from low to high-value
        public int ItemCategoryId { get; set; }                         // foreign key linking ItemCategory table
        public ItemCategory? ItemCategory { get; set; }
        public ICollection<VisitItem>? VisitItems { get; set; }     // one-to-many relationship between VisitItem and Item tables
    }
}
