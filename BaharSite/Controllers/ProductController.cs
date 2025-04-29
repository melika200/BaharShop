using BaharShop.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BaharSite.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductServices _productService;

        public ProductController(ProductServices productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index() 
        {
            var products = await _productService.GetProducts();
            return View(products);
        }

        public async Task<IActionResult> Details(long id) 
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> ProductPaginationPageNavbar(int page = 1, int pageSize = 4, string search = null, string sortOrder = "newest")
        {
            var data = await _productService.GetProductPagination(page, pageSize, search, sortOrder);
            ViewBag.Search = search;
            ViewBag.SortOrder = sortOrder;
            ViewBag.PageSize = pageSize;
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> RateProduct(long productId, int rating)
        {
            if (rating < 1 || rating > 5)
            {
                return Json(new { success = false, message = "امتیاز باید بین 1 تا 5 باشد." });
            }

            var result = await _productService.RateProduct(productId, rating);
            if (!result)
            {
                return Json(new { success = false, message = "محصول یافت نشد." });
            }

            // دریافت محصول به‌روز شده برای میانگین جدید
            var updatedProduct = await _productService.GetProductById(productId);
            return Json(new { success = true, message = "امتیاز با موفقیت ثبت شد.", newAverageRating = updatedProduct.AverageRating });
        }

        public IActionResult GetStock(int productId)
        {
            var product = _productService.GetProductById(productId).Result;
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product.Stock);
        }
    }
}
