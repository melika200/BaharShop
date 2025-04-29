using BaharShop.Application.InterfaceRepository;
using Microsoft.AspNetCore.Mvc;

namespace BaharSite.Component
{
    public class ProductInHome : ViewComponent
    {
        private readonly IProductRepository _productRepository;

        public ProductInHome(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count = 6) 
        {
            // دریافت محصولات
            var products = await _productRepository.GetAll();

            // فیلتر کردن محصولات بر اساس موجود بودن و مرتب‌سازی بر اساس تاریخ ایجاد
            var availableProducts = products
                .Where(p => p.IsAvail) // فقط محصولات موجود
                .OrderByDescending(p => p.InsertTime) // مرتب‌سازی بر اساس تاریخ ایجاد
                .Take(count) // محدود کردن به تعداد مشخصی از محصولات
                .ToList();

         
            return View("/Views/Shared/Component/ProductInHome.cshtml", availableProducts);
        }
    }
}
