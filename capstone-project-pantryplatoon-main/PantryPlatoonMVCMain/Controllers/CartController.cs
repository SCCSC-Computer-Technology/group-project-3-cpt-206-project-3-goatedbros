using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;
using Microsoft.AspNetCore.Identity;

namespace PantryPlatoonMVCMain.Controllers
{
    [Authorize] // Only allow authenticated users to use the cart features
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string CartSessionKey = "ShoppingCart";

        public CartController(ApplicationDbContext context, ILogger<CartController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: /Cart
        public IActionResult Index()
        {
            // Get an instance of the cart and return it as the page's model
            ShoppingCart cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int itemId, int quantity = 1)
        {
            try
            {
                // Logging
                _logger.LogInformation($"--- Add to Cart Logs ---");
                _logger.LogInformation($"ItemId: {itemId}, Quantity: {quantity}");
                _logger.LogInformation($"User authenticated: {HttpContext.User.Identity?.IsAuthenticated}");

                var item = await _context.Items
                    .Include(i => i.ItemCategory)
                    .FirstOrDefaultAsync(i => i.ItemId == itemId);

                // Check if item exists
                if (item == null)
                {
                    _logger.LogWarning($"Item not found with ID: {itemId}"); // Log item not found
                    TempData["ErrorMessage"] = "Item not found.";
                    return RedirectToAction("Index", "Inventory");
                }

                // Log item found
                _logger.LogInformation($"Found item: {item.ItemName}");

                // Check available inventory
                var userCampusId = await GetUserCampusId();
                _logger.LogInformation($"User campus ID: {userCampusId}");

                var availableQuantity = await GetAvailableQuantity(itemId, userCampusId);
                _logger.LogInformation($"Available quantity: {availableQuantity}");

                // Check for stock
                if (availableQuantity <= 0)
                {
                    TempData["ErrorMessage"] = $"{item.ItemName} is currently out of stock.";
                    return RedirectToAction("Index", "Inventory");
                }

                // Create new cart object
                ShoppingCart cart = GetCart();
                _logger.LogInformation($"Current cart has {cart.TotalItems} items");

                var currentInCart = cart.Items.FirstOrDefault(x => x.ItemId == itemId)?.Quantity ?? 0;
                _logger.LogInformation($"Current quantity in cart for this item: {currentInCart}");

                // Check if there are enough items
                if (currentInCart + quantity > availableQuantity)
                {
                    var errorMsg = $"Not enough {item.ItemName} available. Available: {availableQuantity}, In cart: {currentInCart}";
                    _logger.LogWarning(errorMsg);
                    TempData["ErrorMessage"] = errorMsg;
                    return RedirectToAction("Index", "Inventory");
                }

                _logger.LogInformation("Adding item to cart...");
                cart.AddItem(item, quantity);
                _logger.LogInformation($"Cart now has {cart.TotalItems} items");

                SaveCart(cart);
                _logger.LogInformation("Cart saved to session");

                // Verify the save worked by getting an instance of the cart again
                ShoppingCart verifyCart = GetCart();
                _logger.LogInformation($"Verification: Retrieved cart has {verifyCart.TotalItems} items");

                TempData["SuccessMessage"] = $"Added {quantity} {item.ItemName} to your bag.";

                // Redirect back to the same page to refresh the cart count
                string referer = Request.Headers["Referer"].ToString();
                _logger.LogInformation($"Referer: {referer}");

                if (!string.IsNullOrEmpty(referer) && Url.IsLocalUrl(referer))
                {
                    _logger.LogInformation("Redirecting to original referer");
                    return Redirect(referer);
                }

                _logger.LogInformation("Redirecting to Inventory index");
                return RedirectToAction("Index", "Inventory");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                TempData["ErrorMessage"] = "An error occurred while adding the item to your bag.";
                return RedirectToAction("Index", "Inventory");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int itemId, int quantity)
        {
            try
            {
                if (quantity < 1)
                {
                    return await RemoveItem(itemId);
                }

                // Check available inventory
                var userCampusId = await GetUserCampusId();
                var availableQuantity = await GetAvailableQuantity(itemId, userCampusId);

                if (quantity > availableQuantity)
                {
                    TempData["ErrorMessage"] = $"Not enough items available. Available: {availableQuantity}";
                    return RedirectToAction("Index");
                }

                ShoppingCart cart = GetCart();
                cart.UpdateQuantity(itemId, quantity);
                SaveCart(cart);

                TempData["SuccessMessage"] = "Bag updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart quantity");
                TempData["ErrorMessage"] = "Failed to update bag.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            try
            {
                ShoppingCart cart = GetCart();
                cart.RemoveItem(itemId);
                SaveCart(cart);

                TempData["SuccessMessage"] = "Item removed from your bag.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart");
                TempData["ErrorMessage"] = "Failed to remove item.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            try
            {
                ShoppingCart cart = GetCart();

                if (!cart.Items.Any())
                {
                    TempData["ErrorMessage"] = "Your bag is empty.";
                    return RedirectToAction("Index");
                }

                // Ensure items are still available before checkout
                var userCampusId = await GetUserCampusId();
                foreach (var cartItem in cart.Items)
                {
                    var availableQuantity = await GetAvailableQuantity(cartItem.ItemId, userCampusId);
                    if (cartItem.Quantity > availableQuantity)
                    {
                        TempData["ErrorMessage"] = $"Sorry, {cartItem.ItemName} stock has changed. Available: {availableQuantity}, but you have {cartItem.Quantity} in your bag.";
                        return RedirectToAction("Index");
                    }
                }

                // Get current user
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction("Index");
                }

                // Create new Visit object
                var visit = new Visit
                {
                    UserId = user.Id, // Use ApplicationUser.Id (string)
                    CampusId = userCampusId,
                    VisitDate = DateOnly.FromDateTime(DateTime.Now),
                    TotalWeight = cart.TotalWeight,
                    Notes = $"Items taken: {cart.TotalItems} | User: {user.FullName} ({user.Email})"
                };

                _context.Visits.Add(visit);
                await _context.SaveChangesAsync();

                // Create visit items and update inventory
                foreach (var cartItem in cart.Items)
                {
                    // Add visit item
                    var visitItem = new VisitItem
                    {
                        VisitId = visit.VisitId,
                        ItemId = cartItem.ItemId,
                        QuantityTaken = cartItem.Quantity
                    };
                    _context.VisitItems.Add(visitItem);

                    // Update inventory
                    await UpdateInventory(cartItem.ItemId, userCampusId, -cartItem.Quantity);
                }

                await _context.SaveChangesAsync();

                // Clear cart
                cart.Clear();
                SaveCart(cart);

                TempData["SuccessMessage"] = "Visit completed successfully! Thank you for using the pantry.";
                return RedirectToAction("VisitConfirmation", new { visitId = visit.VisitId, isNewVisit = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during checkout");
                TempData["ErrorMessage"] = "An error occurred during checkout. Please try again.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> VisitConfirmation(int visitId, bool isNewVisit)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var visit = await _context.Visits
                .Include(v => v.VisitItems)
                    .ThenInclude(vi => vi.Item)
                        .ThenInclude(i => i.ItemCategory)
                .Include(v => v.User) // Include user information
                .Include(v => v.Campus) // Include campus information
                .FirstOrDefaultAsync(v => v.VisitId == visitId && v.UserId == currentUser.Id);

            if (visit == null)
            {
                return NotFound();
            }

            // Get static content for visit confirmation page
            var visitInfoPage = await _context.StaticPages
                .FirstOrDefaultAsync(sp => sp.PageName == "VisitConfirmation");

            // Pass static content via ViewBag
            ViewBag.VisitInfoContent = visitInfoPage?.HtmlContent;
            ViewBag.isNewVisit = isNewVisit;

            return View(visit);
        }

        public JsonResult GetCartItemCount()
        {
            try
            {
                ShoppingCart cart = GetCart();
                return Json(cart.TotalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart count");
                return Json(0);
            }
        }

        #region Private Helper Methods

        private ShoppingCart GetCart()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new ShoppingCart();
            }

            try
            {
                return JsonConvert.DeserializeObject<ShoppingCart>(cartJson) ?? new ShoppingCart();
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Error deserializing cart from session");
                // Clear corrupted session data
                HttpContext.Session.Remove(CartSessionKey);
                return new ShoppingCart();
            }
        }

        private void SaveCart(ShoppingCart cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            _logger.LogInformation($"Saving cart to session: {cartJson}");

            HttpContext.Session.SetString(CartSessionKey, cartJson);

            // Verify it was saved
            var verification = HttpContext.Session.GetString(CartSessionKey);
            _logger.LogInformation($"Verification - cart retrieved from session: {verification ?? "NULL"}");
        }

        private async Task<int> GetAvailableQuantity(int itemId, int? campusId)
        {
            _logger.LogInformation($"Getting available quantity for itemId: {itemId}, campusId: {campusId}");

            // First try to get from Inventory table (proper way)
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ItemId == itemId && i.CampusId == campusId);

            if (inventory != null)
            {
                _logger.LogInformation($"Found inventory record: Quantity = {inventory.Quantity}");
                return inventory.Quantity;
            }

            // Fallback to Item.Quantity if no inventory record exists (for testing/transition)
            _logger.LogInformation("No inventory record found, checking Item.Quantity");
            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == itemId);

            if (item?.Quantity != null)
            {
                _logger.LogInformation($"Using Item.Quantity as fallback: {item.Quantity}");
                return item.Quantity.Value;
            }

            _logger.LogWarning($"No quantity found for itemId: {itemId}");
            return 0;
        }

        private async Task UpdateInventory(int itemId, int? campusId, int quantityChange)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ItemId == itemId && i.CampusId == campusId);

            if (inventory != null)
            {
                var oldQuantity = inventory.Quantity;
                inventory.Quantity += quantityChange;
                inventory.LastUpdated = DateTime.Now;

                if (inventory.Quantity < 0)
                {
                    inventory.Quantity = 0; // Prevent negative inventory
                }

                _logger.LogInformation($"Updated inventory for ItemId {itemId}: {oldQuantity} -> {inventory.Quantity}");
            }
            else
            {
                _logger.LogWarning($"No inventory record found to update for ItemId {itemId}, CampusId {campusId}");
            }
        }

        private async Task<int?> GetUserCampusId()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.CampusId ?? 1; // Default to campus 1 if not set
        }

        #endregion

        #region Test and Debug Actions (Remove in production)

        // Add this temporary action to your CartController for testing
        public IActionResult TestSession()
        {
            // Test session
            HttpContext.Session.SetString("TestKey", "TestValue");
            var testValue = HttpContext.Session.GetString("TestKey");

            _logger.LogInformation($"Session test - Set: TestValue, Retrieved: {testValue ?? "NULL"}");

            // Test cart specifically
            var cart = new ShoppingCart();
            var testItem = new Item { ItemId = 999, ItemName = "Test Item" };
            cart.AddItem(testItem, 1);

            var cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("ShoppingCart", cartJson);

            var retrievedJson = HttpContext.Session.GetString("ShoppingCart");
            _logger.LogInformation($"Cart test - Set: {cartJson}, Retrieved: {retrievedJson ?? "NULL"}");

            return Json(new
            {
                SessionWorking = testValue == "TestValue",
                CartJson = retrievedJson,
                IsAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false
            });
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("ShoppingCart");
            TempData["SuccessMessage"] = "Bag cleared.";
            return RedirectToAction("Index", "Inventory");
        }

        #endregion
    }
}