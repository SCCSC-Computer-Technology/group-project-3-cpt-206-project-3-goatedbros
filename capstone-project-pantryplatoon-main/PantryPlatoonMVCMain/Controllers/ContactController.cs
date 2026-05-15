using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;
using PantryPlatoonMVCMain.ViewModels;

namespace PantryPlatoonMVCMain.Controllers
{
    // This is the controller for the "Contact" page.
    // It will likely list helpful information about how to contact staff at the SCC pantry.
    public class ContactController : Controller
    {

        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;
        private String _staticPageName = "Contact";

        public ContactController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch the "Contact" static page from database
            var contactPage = await _context.StaticPages
                .FirstOrDefaultAsync(p => p.PageName == _staticPageName);

            return View(contactPage); // Pass the StaticPage to the view
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
