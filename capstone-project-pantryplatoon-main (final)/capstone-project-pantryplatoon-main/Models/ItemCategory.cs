// Capstone 2025 Project
// Pantry Platoon
// Team Members: Michael Lee, Colin Gaffney
// Jared Daniels, Aaron Rash, Donn Gerald

namespace Capstone2025_PantryPlatoon.Models
{
    // model
    public class ItemCategory
    {
        public int ItemCategoryId { get; set; }             // primary key
        public string? ItemCategoryName { get; set; }       // e.g. Food, Hygiene, Baby products

        public ICollection<Item>? Items { get; set; }   // one-to-many relationship between ItemCategory and Item tables
    }
}
