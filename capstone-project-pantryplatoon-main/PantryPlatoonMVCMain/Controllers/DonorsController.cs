
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;
using PantryPlatoonMVCMain.ViewModels;

namespace PantryPlatoonMVCMain.Controllers
{
    //[Authorize(Roles = "Admin,User")]
    public class DonorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public DonorsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Donors
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int? campusId)
        {
            var donationsQuery = _context.Donations
                .Include(d => d.Donor)
                .Include(d => d.Campus) // Optional, if you want to display campus name
                .AsQueryable();

            // Safely filter by CampusId if provided
            if (campusId.HasValue)
            {
                donationsQuery = donationsQuery
                    .Where(d => d.CampusId.HasValue && d.CampusId.Value == campusId.Value);
            }

            var viewModel = new DonorIndexViewModel
            {
                Donations = await donationsQuery
                    .OrderByDescending(d => d.DateDonated)
                    .ToListAsync(),
                Campuses = await _context.Campuses.ToListAsync(),
                CampusId = campusId
            };

            return View(viewModel);

        }

        // GET: Donors/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var donor = await _context.Donors
                .Include(d => d.Donations)
                .FirstOrDefaultAsync(d => d.DonorId == id);

            if (donor == null) return NotFound();

            return View(donor);
        }

        // GET: Donors/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            var items = _context.Items.Include(i => i.ItemCategory).ToList();
            var campuses = _context.Campuses.ToList();

            var viewModel = new DonorDonationViewModel
            {
                Categories = _context.ItemCategories.ToList(),
                Campuses = campuses,
                CampusId = campuses.Count == 1 ? campuses[0].CampusId : null,
                Items = _context.Items.ToList(),
                Donations = new List<Donation> { new Donation() },
                ItemEntries = items.Select(i => new ItemEntry
                {
                    ItemName = i.ItemName,
                    ItemCategoryId = i.ItemCategory.ItemCategoryId
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Donors/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonorDonationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Creates new Donor entry
                _context.Donors.Add(viewModel.Donor);
                await _context.SaveChangesAsync();

                foreach (var donation in viewModel.Donations)
                {
                    if (!string.IsNullOrWhiteSpace(donation.ItemName) && donation.Quantity > 0)
                    {
                        donation.DonorId = viewModel.Donor.DonorId;
                        donation.DateDonated = DateTime.Now;
                        donation.CampusId = viewModel.CampusId;
                        _context.Donations.Add(donation);

                        // Checks to see if item already exists
                        var existingItem = await _context.Items
                        .FirstOrDefaultAsync(i => i.ItemName == donation.ItemName);
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("ThankYou");
            }
            else
            {
                Console.WriteLine("Form not valid.");
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Field: {entry.Key}, Error: {error.ErrorMessage}");
                    }
                }

                viewModel.Campuses = _context.Campuses.ToList();
                viewModel.Categories = _context.ItemCategories.ToList();
                viewModel.Items = _context.Items.ToList();

                var items = _context.Items.Include(i => i.ItemCategory).ToList();
                viewModel.ItemEntries = items.Select(i => new ItemEntry
                {
                    ItemName = i.ItemName,
                    ItemCategoryId = i.ItemCategory.ItemCategoryId
                }).ToList();

                return View(viewModel);
            }
        }

        // GET: Donors/Edit/
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donor = await _context.Donors.FindAsync(id);
            if (donor == null)
            {
                return NotFound();
            }
            return View(donor);
        }

        // POST: Donors/Edit/
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonorId,DonorName,Email")] Donor donor)
        {
            if (id != donor.DonorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonorExists(donor.DonorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(donor);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkAsReceived(int id)
        {
            var donation = await _context.Donations.FirstOrDefaultAsync(d => d.DonationId == id);
            if (donation == null) return NotFound();

            // Only mark as received if it hasn't been already
            if (!donation.IsReceived)
            {
                donation.IsReceived = true;

                
                var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemName == donation.ItemName);
                if (item != null)
                {
                    // Update Item Table
                    item.Quantity += donation.Quantity ?? 0;

                    var inventoryEntry = await _context.Inventories
                        .FirstOrDefaultAsync(i => i.ItemId == item.ItemId && i.CampusId == donation.CampusId);

                    if (inventoryEntry != null)
                    {
                        inventoryEntry.Quantity += donation.Quantity ?? 0;
                        inventoryEntry.LastUpdated = DateTime.Now;
                    }
                    else
                    {
                        _context.Inventories.Add(new Inventory
                        {
                            ItemId = item.ItemId,
                            CampusId = donation.CampusId ?? 0,
                            Quantity = donation.Quantity ?? 0,
                            LastUpdated = DateTime.Now
                        });
                    }
                }

                

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = donation.DonorId });
        }


        // GET: Donors/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donor = await _context.Donors
                .FirstOrDefaultAsync(m => m.DonorId == id);
            if (donor == null)
            {
                return NotFound();
            }

            return View(donor);
        }

        // POST: Donors/Delete
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donor = await _context.Donors.FindAsync(id);
            if (donor != null)
            {
                _context.Donors.Remove(donor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonorExists(int id)
        {
            return _context.Donors.Any(e => e.DonorId == id);
        }

        [AllowAnonymous]
        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
