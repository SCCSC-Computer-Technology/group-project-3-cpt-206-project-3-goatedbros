using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;
using PantryPlatoonMVCMain.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryPlatoonMVCMain.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext _context;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _context = context;
        }

        // Admin Index showing Identity users
        public async Task<IActionResult> Index()
        {
            var users = new List<UserDisplayViewModel>();

            foreach (var user in userManager.Users.ToList())
            {
                var roleNames = await userManager.GetRolesAsync(user);

                var visits = await _context.Visits
                .Where(v => v.UserId == user.Id)
                .Include(v => v.VisitItems)
                    .ThenInclude(vi => vi.Item)
                .OrderByDescending(v => v.VisitDate)
                .ToListAsync();
                var displayUser = new UserDisplayViewModel
                {
                    IdentityId = user.Id,
                    Email = user.Email ?? "",
                    RoleNames = roleNames,
                    UserId = user.Id,                    // error right here
                    SCCId = int.TryParse(user.SCCId, out int sccIdVal) ? sccIdVal : (int?)null,
                    FirstName = user.FirstName ?? "(not found)",
                    LastName = user.LastName ?? "",
                    VisitHistory = visits.Select(v => new VisitDisplayViewModel
                    {
                        VisitDate = v.VisitDate,
                        Items = v.VisitItems.Select(vi => new VisitItemDisplayViewModel
                        {
                            ItemName = vi.Item?.ItemName ?? "(unknown)",
                            Quantity = vi.Quantity,
                            Weight = vi.Weight
                        }).ToList()
                    }).ToList()
                };
                


                users.Add(displayUser);
            }

            var model = new UserDisplayViewModelHolder
            {
                Users = users,
                Roles = roleManager.Roles
            };

            return View(model);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<ApplicationUser> users = new List<ApplicationUser>();

            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            user.RoleNames = await userManager.GetRolesAsync(user);
            users.Add(user);

            UserViewModel model = new UserViewModel
            {
                Users = users,
                Roles = roleManager.Roles
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await userManager.Users
                .Include(u => u.Visits)
                    .ThenInclude(v => v.VisitItems)
                        .ThenInclude(vi => vi.Item)
                .Include(u => u.Campus)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
                    TempData["message"] = errorMessage;
                }
            }
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> Donors()
        {
            // Get all users in the "Donor" role
            var donors = await userManager.GetUsersInRoleAsync("Donor");

            // Map to your view model
            var donorViewModels = donors.Select(d => new UserDisplayViewModel
            {
                IdentityId = d.Id,                
                FirstName = d.FirstName ?? "(no first name)",    // kept getting warning here
                LastName = d.LastName ?? "(no last name)",       // warning
                Email = d.Email ?? "(no email)",                // warning

            }).ToList();

            return View(donorViewModels);
        }


        [HttpPost]
        public async Task<IActionResult> AddtoRole(string userID, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                TempData["message"] = $"Role '{roleName}' doesn't exist.";
            }
            else
            {
                var user = await userManager.FindByIdAsync(userID);
                if (user != null)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string userID, string roleName)
        {
            var user = await userManager.FindByIdAsync(userID);
            if (user != null)
            {
                await userManager.RemoveFromRoleAsync(user, roleName);
            }
            return RedirectToAction("Index", "Admin");
        }
    }
}
