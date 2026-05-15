// Capstone 2025 Project
// Pantry Platoon
// Team Members: Michael Lee, Colin Gaffney
// Jared Daniels, Aaron Rash, Donn Gerald

namespace Capstone2025_PantryPlatoon.Models
{
    public class AdminReports
    {
        public int ReportId { get; set; }               // primary key
        public DateTime Month { get; set; }             // monthly report
        public int TotalVisits { get; set; }            // total number of visits to the pantry
        public int TotalStudentsServed { get; set; }    // total number of students served
        public int RepeatVisitors { get; set; }         // how many visitors visited more than once
        public int TotalItems { get; set; }             // total number of items distributed
        public decimal TotalWeight { get; set; }        // total weight of items that were distributed
    }
}
