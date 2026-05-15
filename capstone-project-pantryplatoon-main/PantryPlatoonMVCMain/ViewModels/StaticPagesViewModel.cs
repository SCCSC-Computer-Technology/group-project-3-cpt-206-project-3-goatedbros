using PantryPlatoonMVCMain.Models;

namespace PantryPlatoonMVCMain.ViewModels
{
    public class StaticPagesViewModel
    {
        public List<StaticPage> MainPages { get; set; } = new List<StaticPage>();
        public List<StaticPage> SubsectionPages { get; set; } = new List<StaticPage>();
    }
}
