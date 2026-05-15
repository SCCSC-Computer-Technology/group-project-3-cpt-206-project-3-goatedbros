using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;
using PantryPlatoonMVCMain.ViewModels;

namespace PantryPlatoonMVCMain.Controllers
{
    // This is the controller for the "About" page.
    // It will likely list helpful information about the SCC pantry not found on the home page.
    public class AboutController : Controller
    {

        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;
        private String _staticPageName = "About";

        public AboutController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger; // Initalize logger
            _context = context; // Initialize database context
        }

        public async Task<IActionResult> Index()
        {
            // Fetch the "About" static page from database
            var aboutPage = await _context.StaticPages
                .FirstOrDefaultAsync(p => p.PageName == _staticPageName);

            return View(aboutPage); // Pass the StaticPage to the view
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
