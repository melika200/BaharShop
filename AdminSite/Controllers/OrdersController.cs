using BaharShop.Application.Services;
using BaharShop.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ShoppingCartService _ShoppingCartService;
        public OrdersController(ShoppingCartService ShoppingCartService)
        {
            _ShoppingCartService = ShoppingCartService;
        }
        // نمایش صفحه‌ای است که لیست همه سفارش‌ها را نشان می‌دهد.
        public async Task<IActionResult> Index()
        {
            var data = await _ShoppingCartService.GetAllOrders();
            return View(data);
        }
        //نمایش صفحه ویرایش یا جزئیات یک سفارش خاص 
        public async Task<IActionResult> Edit(long id)
        {
            var data = await _ShoppingCartService.GetOrderWithId(id);
            return View(data);
        }



        [HttpPost]
        //تغییر وضعیت یک سفارش 
        public async Task<IActionResult> SetStatusCommand([FromBody] StatusDto model)
        {
            var data = await _ShoppingCartService.SetStatus(model.Id, model.State);
            return Ok(new { res = data });
        }

    }
}
