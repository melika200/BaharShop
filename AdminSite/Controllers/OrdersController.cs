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
        public async Task<IActionResult> Index()
        {
            var data = await _ShoppingCartService.GetAllOrders();
            return View(data);
        }
        public async Task<IActionResult> Edit(long id)
        {
            var data = await _ShoppingCartService.GetOrderWithId(id);
            return View(data);
        }



        [HttpPost]
        public async Task<IActionResult> SetStatusCommand([FromBody] StatusDto model)
        {
            var data = await _ShoppingCartService.SetStatus(model.Id, model.State);
            return Ok(new { res = data });
        }

    }
}
