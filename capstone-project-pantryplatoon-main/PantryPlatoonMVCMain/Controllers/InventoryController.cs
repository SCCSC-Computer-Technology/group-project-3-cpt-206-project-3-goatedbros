using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;
using PantryPlatoonMVCMain.Util;
using PantryPlatoonMVCMain.ViewModels;

namespace PantryPlatoonMVCMain.Controllers
{
    [Authorize] // Students need to be logged in to shop
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<InventoryController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InventoryController(ApplicationDbContext context, ILogger<InventoryController> logger, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Inventory (This is the student-facing shopping page)
        public async Task<IActionResult> Index(int? page, string sortBy = "category", string category = "", string search = "", int? campusId = null)
        {
            const int pageSize = 12;
            int pageNumber = page ?? 1;

            var user = await _userManager.GetUserAsync(User);
            int selectedCampusId = await GetSelectedCampusId(campusId, user);

            // Debugging category filters
            Console.WriteLine($"Category filter: '{category}'");
            Console.WriteLine($"Search filter: '{search}'");

            // Get all inventory items for this campus that have quantity > 0
            var availableItemsQuery = _context.Inventories
                .Include(i => i.Item)
                    .ThenInclude(item => item.ItemCategory)
                .Where(i => i.CampusId == selectedCampusId && i.Quantity > 0);

            // Apply category filter
            if (!string.IsNullOrEmpty(category))
            {
                Console.WriteLine($"Applying category filter for: '{category}'");
                availableItemsQuery = availableItemsQuery.Where(i => i.Item!.ItemCategory!.ItemCategoryName == category);

                var countAfterCategoryFilter = await availableItemsQuery.CountAsync();
                Console.WriteLine($"Items after category filter: {countAfterCategoryFilter}");
            }

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                availableItemsQuery = availableItemsQuery.Where(i =>
                    i.Item!.ItemName.Contains(search) ||
                    (i.Item.Description != null && i.Item.Description.Contains(search)));
            }

            // Send data to InventoryItemViewModel
            var itemsQuery = availableItemsQuery.Select(i => new InventoryItemViewModel
            {
                InventoryId = i.InventoryId,
                ItemId = i.Item!.ItemId,
                ItemName = i.Item.ItemName,
                Description = i.Item.Description,
                CategoryName = i.Item.ItemCategory!.ItemCategoryName,
                AvailableQuantity = i.Quantity,
                Weight = i.Item.Weight,
                Points = i.Item.Points,
                LastUpdated = i.LastUpdated,
                ImagePath = i.Item.ImagePath
            });

            // Apply sorting
            itemsQuery = ApplySorting(itemsQuery, sortBy);

            var allCampuses = await _context.Campuses
                .OrderBy(c => c.CampusName)
                .ToListAsync();

            var paginatedItems = await PaginatedList<InventoryItemViewModel>.CreateAsync(
                itemsQuery, pageNumber, pageSize);

            // Get categories for filter dropdown
            var categories = await _context.ItemCategories
                .OrderBy(c => c.ItemCategoryName)
                .Select(c => c.ItemCategoryName)
                .ToListAsync();

            var selectedCampus = await _context.Campuses.FindAsync(selectedCampusId);

            // Debug: Available categories
            Console.WriteLine($"Available categories: {string.Join(", ", categories)}");

            ViewData["CampusName"] = selectedCampus?.CampusName ?? "Unknown Campus";
            ViewData["SelectedCampusId"] = selectedCampusId;
            ViewData["AllCampuses"] = allCampuses;
            ViewData["Categories"] = new SelectList(categories, category);
            ViewData["CurrentSort"] = sortBy;
            ViewData["CurrentCategory"] = category;
            ViewData["CurrentSearch"] = search;

            return View(paginatedItems);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeCampus(int campusId, string returnUrl = "")
        {
            // Validate that the campus exists
            var campus = await _context.Campuses.FindAsync(campusId);
            if (campus == null)
            {
                TempData["ErrorMessage"] = "Invalid campus selection.";
                return RedirectToAction("Index");
            }

            // Save campus selection as cookie (expires in 30 days)
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax
            };

            Response.Cookies.Append("SelectedCampusId", campusId.ToString(), cookieOptions);

            TempData["SuccessMessage"] = $"Campus changed to {campus.CampusName}";

            // Redirect back to where they came from, or Index if no return URL
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }


        // GET: Inventory/Details/5
        public async Task<IActionResult> Details(int id, int? campusId = null)
        {
            var user = await _userManager.GetUserAsync(User);
            int selectedCampusId = await GetSelectedCampusId(campusId, user);

            var inventoryItem = await _context.Inventories
                .Include(i => i.Item)
                    .ThenInclude(item => item.ItemCategory)
                .FirstOrDefaultAsync(i => i.ItemId == id && i.CampusId == selectedCampusId);

            if (inventoryItem == null || inventoryItem.Quantity <= 0)
            {
                return NotFound();
            }

            // Pass campus info to view
            var selectedCampus = await _context.Campuses.FindAsync(selectedCampusId);
            var allCampuses = await _context.Campuses.OrderBy(c => c.CampusName).ToListAsync();

            ViewData["CampusName"] = selectedCampus?.CampusName ?? "Unknown Campus";
            ViewData["SelectedCampusId"] = selectedCampusId;
            ViewData["AllCampuses"] = allCampuses;

            return View(inventoryItem);
        }

        // Short functionality
        private IQueryable<InventoryItemViewModel> ApplySorting(IQueryable<InventoryItemViewModel> query, string sortBy)
        {
            return sortBy.ToLower() switch
            { // Uses different URL tags to sort inventory items
                "name-asc" => query.OrderBy(i => i.ItemName),
                "name-desc" => query.OrderByDescending(i => i.ItemName),
                "quantity-asc" => query.OrderBy(i => i.AvailableQuantity).ThenBy(i => i.ItemName),
                "quantity-desc" => query.OrderByDescending(i => i.AvailableQuantity).ThenBy(i => i.ItemName),
                "points-asc" => query.OrderBy(i => i.Points ?? 0).ThenBy(i => i.ItemName),
                "points-desc" => query.OrderByDescending(i => i.Points ?? 0).ThenBy(i => i.ItemName),
                "updated" => query.OrderByDescending(i => i.LastUpdated).ThenBy(i => i.ItemName),
                "category" or _ => query.OrderBy(i => i.CategoryName).ThenByDescending(i => i.AvailableQuantity).ThenBy(i => i.ItemName)
            };
        }

        private async Task<int> GetSelectedCampusId(int? requestedCampusId, ApplicationUser? user)
        {
            // Priority:
            // 1. Explicitly requested campus ID (from URL arguments)
            // 2. Campus from cookie
            // 3. User's default campus
            // 4. First available campus
            // 5. Default to 1

            if (requestedCampusId.HasValue)
            {
                var requestedCampus = await _context.Campuses.FindAsync(requestedCampusId.Value);
                if (requestedCampus != null)
                {
                    // Save this selection as cookie for future visits
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(30),
                        HttpOnly = true,
                        Secure = Request.IsHttps,
                        SameSite = SameSiteMode.Lax
                    };
                    Response.Cookies.Append("SelectedCampusId", requestedCampusId.Value.ToString(), cookieOptions);
                    return requestedCampusId.Value;
                }
            }

            // Check cookie
            if (Request.Cookies.TryGetValue("SelectedCampusId", out string? cookieValue) &&
                int.TryParse(cookieValue, out int cookieCampusId))
            {
                var cookieCampus = await _context.Campuses.FindAsync(cookieCampusId);
                if (cookieCampus != null)
                {
                    return cookieCampusId;
                }
            }

            // Use user's default campus
            if (user?.CampusId != null)
            {
                var userCampus = await _context.Campuses.FindAsync(user.CampusId);
                if (userCampus != null)
                {
                    return user.CampusId.Value;
                }
            }

            // Get first available campus
            var firstCampus = await _context.Campuses.OrderBy(c => c.CampusName).FirstOrDefaultAsync();
            if (firstCampus != null)
            {
                return firstCampus.CampusId;
            }

            // Fallback
            return 1;
        }

        #region Campus Management

        // GET: Inventory/Manage
        [Authorize(Roles = "Admin,Volunteer")] // Only SCC staff can manage inventory
        public async Task<IActionResult> Manage(string? category, string? stockFilter, string? search, string? sortBy, string? sortOrder, int? campusId = null)
        {
            var user = await _userManager.GetUserAsync(User);
            int selectedCampusId = await GetSelectedCampusId(campusId, user);

            // Base query - get ALL items and show their stock for the selected campus
            var query = _context.Items
                .Include(i => i.ItemCategory)
                .Select(i => new InventoryManagementViewModel
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    CategoryName = i.ItemCategory!.ItemCategoryName ?? "Uncategorized",
                    CurrentQuantity = i.Inventories
                        .Where(inv => inv.CampusId == selectedCampusId)
                        .Select(inv => inv.Quantity)
                        .FirstOrDefault(), // This will be 0 if no inventory record exists for this campus
                    LastUpdated = i.Inventories
                        .Where(inv => inv.CampusId == selectedCampusId)
                        .Select(inv => inv.LastUpdated)
                        .FirstOrDefault(),
                    ImagePath = i.ImagePath
                });

            // Add filters
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(i => i.CategoryName == category);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(i => i.ItemName.Contains(search));
            }

            // Add stock filter
            if (!string.IsNullOrEmpty(stockFilter))
            {
                switch (stockFilter)
                {
                    case "in-stock":
                        query = query.Where(i => i.CurrentQuantity > 0);
                        break;
                    case "out-of-stock":
                        query = query.Where(i => i.CurrentQuantity == 0);
                        break;
                    case "low-stock":
                        query = query.Where(i => i.CurrentQuantity > 0 && i.CurrentQuantity <= 5);
                        break;
                }
            }

            // Apply sorting
            switch (sortBy?.ToLower())
            {
                case "name":
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(i => i.ItemName)
                        : query.OrderBy(i => i.ItemName);
                    break;
                case "category":
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(i => i.CategoryName).ThenBy(i => i.ItemName)
                        : query.OrderBy(i => i.CategoryName).ThenBy(i => i.ItemName);
                    break;
                case "quantity":
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(i => i.CurrentQuantity).ThenBy(i => i.ItemName)
                        : query.OrderBy(i => i.CurrentQuantity).ThenBy(i => i.ItemName);
                    break;
                case "updated":
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(i => i.LastUpdated).ThenBy(i => i.ItemName)
                        : query.OrderBy(i => i.LastUpdated).ThenBy(i => i.ItemName);
                    break;
                default:
                    // Default sort by category then name
                    query = query.OrderBy(i => i.CategoryName).ThenBy(i => i.ItemName);
                    break;
            }

            var inventoryData = await query.ToListAsync();

            // Retrieve all categories (not just those at this campus)
            var allCategories = await _context.Items
                .Include(i => i.ItemCategory)
                .Select(i => i.ItemCategory!.ItemCategoryName ?? "Uncategorized")
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            // Get all campuses for campus selector
            var allCampuses = await _context.Campuses
                .OrderBy(c => c.CampusName)
                .ToListAsync();

            var selectedCampus = await _context.Campuses.FindAsync(selectedCampusId);

            // Send data to view
            ViewData["CampusName"] = selectedCampus?.CampusName ?? "Unknown Campus";
            ViewData["SelectedCampusId"] = selectedCampusId;
            ViewData["AllCampuses"] = allCampuses;
            ViewData["Categories"] = allCategories;
            ViewData["CurrentCategory"] = category;
            ViewData["CurrentStockFilter"] = stockFilter;
            ViewData["CurrentSearch"] = search;
            ViewData["CurrentSortBy"] = sortBy;
            ViewData["CurrentSortOrder"] = sortOrder;

            return View(inventoryData);
        }

        // POST: Update inventory quantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Volunteer")]
        public async Task<IActionResult> UpdateQuantity(int itemId, int quantity, string? returnUrl, int? campusId = null)
        {
            try
            {
                // Check if quantity is < 0
                if (quantity < 0)
                {
                    TempData["ErrorMessage"] = "Quantity cannot be negative.";
                    return RedirectToReturnUrl(returnUrl);
                }

                // Get user and determine campus ID
                var user = await _userManager.GetUserAsync(User);
                int selectedCampusId = await GetSelectedCampusId(campusId, user);

                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.ItemId == itemId && i.CampusId == selectedCampusId);

                var item = await _context.Items.FindAsync(itemId);
                var itemName = item?.ItemName ?? "Unknown Item";

                if (inventory == null)
                {
                    // Create new inventory record
                    inventory = new Inventory
                    {
                        ItemId = itemId,
                        CampusId = selectedCampusId,
                        Quantity = quantity,
                        LastUpdated = DateTime.Now
                    };
                    _context.Inventories.Add(inventory);
                    _logger.LogInformation($"Created new inventory record for ItemId {itemId}, Quantity {quantity}");
                }
                else
                {
                    // Update existing inventory
                    var oldQuantity = inventory.Quantity;
                    inventory.Quantity = quantity;
                    inventory.LastUpdated = DateTime.Now;
                    _logger.LogInformation($"Updated inventory for ItemId {itemId} from {oldQuantity} to {quantity}");
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Updated {itemName} quantity to {quantity}.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating inventory quantity");
                TempData["ErrorMessage"] = "An error occurred while updating inventory.";
            }

            return RedirectToReturnUrl(returnUrl);
        }

        // POST: Quick adjust quantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Volunteer")]
        public async Task<IActionResult> QuickAdjust(int itemId, int adjustment, string? returnUrl, int? campusId = null)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                int selectedCampusId = await GetSelectedCampusId(campusId, user);

                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.ItemId == itemId && i.CampusId == selectedCampusId);

                var item = await _context.Items.FindAsync(itemId);
                var itemName = item?.ItemName ?? "Unknown Item";

                int newQuantity;

                if (inventory == null)
                {
                    // Create new inventory object
                    newQuantity = Math.Max(0, adjustment);
                    inventory = new Inventory
                    {
                        ItemId = itemId,
                        CampusId = selectedCampusId,
                        Quantity = newQuantity,
                        LastUpdated = DateTime.Now
                    };
                    _context.Inventories.Add(inventory);
                }
                else
                {
                    // Update existing inventory
                    newQuantity = Math.Max(0, inventory.Quantity + adjustment);
                    inventory.Quantity = newQuantity;
                    inventory.LastUpdated = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Adjusted {itemName} by {adjustment:+#;-#;0}. New quantity: {newQuantity}.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adjusting inventory quantity");
                TempData["ErrorMessage"] = "An error occurred while adjusting inventory.";
            }

            return RedirectToReturnUrl(returnUrl);
        }

        // GET: Add new item to inventory
        [Authorize(Roles = "Admin,Volunteer")]
        public async Task<IActionResult> AddNewItem()
        {
            ViewData["Categories"] = await _context.ItemCategories.OrderBy(c => c.ItemCategoryName).ToListAsync();
            return View(new NewItemViewModel());
        }

        // POST: Add new item to inventory
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Volunteer")]
        public async Task<IActionResult> AddNewItem(NewItemViewModel model, int? campusId = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if item already exists
                    var existingItem = await _context.Items
                        .FirstOrDefaultAsync(i => i.ItemName.ToLower() == model.ItemName.ToLower());

                    if (existingItem != null)
                    {
                        ModelState.AddModelError("ItemName", "An item with this name already exists.");
                        ViewData["Categories"] = await _context.ItemCategories.OrderBy(c => c.ItemCategoryName).ToListAsync();
                        return View(model);
                    }

                    var item = new Item
                    {
                        ItemName = model.ItemName.Trim(),
                        Description = model.Description?.Trim(),
                        ItemCategoryId = model.ItemCategoryId,
                        Weight = model.Weight,
                        Points = model.Points
                    };

                    // Handle image upload
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        item.ImagePath = await SaveImage(model.ImageFile);
                    }

                    _context.Items.Add(item);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Created new item: {item.ItemName} (ID: {item.ItemId})");

                    // Add initial inventory for current campus
                    if (model.InitialQuantity > 0)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        int selectedCampusId = await GetSelectedCampusId(campusId, user);

                        var inventory = new Inventory
                        {
                            ItemId = item.ItemId,
                            CampusId = selectedCampusId,
                            Quantity = model.InitialQuantity,
                            LastUpdated = DateTime.Now
                        };
                        _context.Inventories.Add(inventory);
                        await _context.SaveChangesAsync();

                        _logger.LogInformation($"Added initial inventory for new item: {model.InitialQuantity} units");
                    }

                    TempData["SuccessMessage"] = $"Successfully added '{item.ItemName}' to the catalog" +
                        (model.InitialQuantity > 0 ? $" with {model.InitialQuantity} initial stock." : ".");

                    return RedirectToAction("Manage");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding new item");
                    ModelState.AddModelError("", "An error occurred while adding the item.");
                }
            }

            ViewData["Categories"] = await _context.ItemCategories.OrderBy(c => c.ItemCategoryName).ToListAsync();
            return View(model);
        }

        private IActionResult RedirectToReturnUrl(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            // Get a new filename
            string newFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "items");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Save file
            string filePath = Path.Combine(uploadPath, newFileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return newFileName;
        }

        #endregion

    }
}