using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PantryPlatoonMVCMain.Models;

namespace PantryPlatoonMVCMain.Views.Shared.Components.CartCount
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly ILogger<CartCountViewComponent> _logger;

        public CartCountViewComponent(ILogger<CartCountViewComponent> logger)
        {
            _logger = logger;
        }

        public IViewComponentResult Invoke()
        {
            try
            {
                int itemCount = 0;

                if (HttpContext.User.Identity?.IsAuthenticated == true)
                {
                    // Get cart from session
                    var cartJson = HttpContext.Session.GetString("ShoppingCart");

                    if (!string.IsNullOrEmpty(cartJson))
                    {
                        try
                        {
                            var cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
                            itemCount = cart?.TotalItems ?? 0;
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogWarning(ex, "Error deserializing cart from session");
                            // Clear corrupted session data
                            HttpContext.Session.Remove("ShoppingCart");
                        }
                    }
                }
                // Note: Only authenticated users can use the pantry, so no guest cart needed

                return View(itemCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart count");
                return View(0);
            }
        }
    }
}