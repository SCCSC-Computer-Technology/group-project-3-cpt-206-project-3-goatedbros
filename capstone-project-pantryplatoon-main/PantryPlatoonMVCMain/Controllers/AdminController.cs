using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.ViewModels;
using PantryPlatoonMVCMain.Models;
using System.Threading.Tasks;

namespace PantryPlatoonMVCMain.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;


        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<IActionResult> Index()
        {
            // Items
            var items = await _context.Items.Include(i => i.ItemCategory).ToListAsync();

            // Users
            var users = new List<ApplicationUser>();
            foreach (var user in _userManager.Users.ToList())
            {
                user.RoleNames = await _userManager.GetRolesAsync(user);
                users.Add(user);
            }

            // Visits
            var visits = await _context.Visits.ToListAsync();

            var UserViewModel = new UserViewModel
            {
                Users = users,
                Roles = _roleManager.Roles
            };

            var donorCount = _context.Donors.Count();

            var viewModel = new AdminViewModel
            {
                Items = items,
                Users = UserViewModel,
                Visits = visits,
                DonorCount = donorCount
            };

            return View(viewModel);
        }
    }
}