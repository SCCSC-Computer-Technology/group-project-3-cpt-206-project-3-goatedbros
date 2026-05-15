// Capstone 2025 Project
// Pantry Platoon
// Team Members: Michael Lee, Colin Gaffney
// Jared Daniels, Aaron Rash, Donn Gerald

namespace Capstone2025_PantryPlatoon.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }        // primary key
        public int Quantity { get; set; }           // how many units per item
        public DateTime LastUpdated { get; set; }   // date/time items were last updated
        public int ItemId { get; set; }             // foreign key linking Inventory and Item tables
        public Item? Item { get; set; }
        public int CampusId { get; set; }           // foreign key linking Inventory and Campus tables
        public Campus? Campus { get; set; }         // e.g. which campus are the items stored?
    }
}
