using BaharShop.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BaharSite.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ShoppingCartService _shoppingCartService;

        public ProfileController(ShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            long userId = Convert.ToInt64(userIdClaim);
            // *THIS IS THE CHANGE*: Get *all* orders for the user. The view will handle filtering.
            var userOrders = await _shoppingCartService.GetUserOrders(userId);

            return View(userOrders);
        }
    }
}
