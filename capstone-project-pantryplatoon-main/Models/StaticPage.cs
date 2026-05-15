// Capstone 2025 Project
// Pantry Platoon
// Team Members: Michael Lee, Colin Gaffney
// Jared Daniels, Aaron Rash, Donn Gerald

namespace Capstone2025_PantryPlatoon.Models
{
    /* This model will enable admin to edit 
     * certain pages like the Home/About 
     * page. Possible configuration for a 
     * dashboard, perhaps?
    */
    public class StaticPage
    {
        public int PageId { get; set; }
        public string? PageName { get; set; }
        public string? HtmlContent { get; set; }
    }
}
