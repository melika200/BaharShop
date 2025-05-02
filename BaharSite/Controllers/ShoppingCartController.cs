using BaharShop.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using BaharShop.Application.Services.Dto;
using BaharSite.ViewModel;

namespace BaharSite.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService _ShoppingCartService;
        public ShoppingCartController(ShoppingCartService shoppingcartService)
        {
            _ShoppingCartService = shoppingcartService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket([FromBody] AddBasketDto model)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Ok(new { res = false, msg = "شما لاگین نکرده‌اید." });
            }

            long userId = Convert.ToInt64(userIdClaim);

            var result = await _ShoppingCartService.AddToBasket(model.productId, model.qty, userId);

            if (result)
            {
                return Ok(new { res = true });
            }
            else
            {
                return Ok(new { res = false, msg = "موجودی محصول کافی نیست یا محصول یافت نشد." });
            }
        }






        [Authorize]
        public async Task<IActionResult> Basket()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Authentication");
            }
            long userId = Convert.ToInt64(userIdClaim);

            var basketItems = await _ShoppingCartService.GetUserBasket(userId);

            var paymentInfo = new PaymentInfoViewModel(); // می‌توانید در صورت نیاز اطلاعات قبلی را بارگذاری کنید

            var model = new BasketViewModel
            {
                Items = basketItems,
                PaymentInfo = paymentInfo
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> RemoveBasket([FromBody] RemoveBasketDto model)
        {

            var res = await _ShoppingCartService.RemoveItemBasket(model.Id);
            return Ok(new { res = true });
        }


        [HttpPost]
        public async Task<IActionResult> UpdateBasketItem([FromBody] UpdateBasketDto model)
        {
            var result = await _ShoppingCartService.UpdateBasketItem(model.Id, model.Quantity);
            if (result != null)
            {
                return Ok(new { res = true, price = result.Price });
            }
            return Ok(new { res = false });
        }



        [Authorize]
        [HttpGet]
        public IActionResult Pay()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            var paymentInfo = new PaymentInfoViewModel();
            return View(paymentInfo);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Pay(PaymentInfoViewModel model)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            long userId = Convert.ToInt64(userIdClaim);

            if (string.IsNullOrEmpty(model.Address) || string.IsNullOrEmpty(model.Mobile))
            {
                ModelState.AddModelError("", "آدرس و موبایل نمی‌توانند خالی باشند.");
                return View(model);
            }

            bool result = await _ShoppingCartService.Pay(model.Mobile, model.Address, userId);

            if (result)
            {
                return RedirectToAction("Index", "Profile");
            }
            else
            {
                ModelState.AddModelError("", "پرداخت انجام نشد، لطفا مجددا تلاش کنید.");
                return View(model);
            }
        }




    }
}
