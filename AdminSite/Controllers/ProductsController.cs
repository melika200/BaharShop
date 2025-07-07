using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaharShop.Domain;
using BaharShop.Persistence.Context;
using BaharShop.Application.Services.Dto;
using BaharShop.Application.Services;

namespace AdminSite.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductServices _productServices;
        private readonly CategoryService _categoryService;
        public ProductsController(ProductServices productServices , CategoryService categoryService)
        {
            _productServices = productServices;
            _categoryService = categoryService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _productServices.GetProducts();
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productServices.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        //کاربر صفحه ایجاد محصول را درخواست می‌کند (مثلاً وقتی روی دکمه "ایجاد محصول جدید" کلیک می‌کند).
        // آماده‌سازی داده‌های لازم برای نمایش فرم است، مثل گرفتن لیست دسته‌بندی‌ها 
        //ویوی فرم ایجاد محصول را نمایش می‌دهد.
        // فقط داده‌ها را دریافت می‌کند و هیچ تغییری در سرور ایجاد نمی‌کند.
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetCategories();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // کاربر فرم ایجاد محصول را پر کرده و دکمه ارسال را زده است.
        //داده‌های ورودی را اعتبارسنجی می‌کند (مثلاً بررسی می‌کند که تصویر بارگذاری شده باشد).
        //داده‌ها معتبر باشند، محصول جدید را در سیستم ایجاد می‌کند
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            // بررسی اینکه آیا تصویر بارگذاری شده است
            if (productDto.Img == null)
            {
                ModelState.AddModelError("Img", "Image is required."); // خطا به مدل اضافه کنید
            }

            if (ModelState.IsValid)
            {
                var categories = await _categoryService.GetCategories();
                ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", productDto.CategoryId);
                return View(productDto);
            }

            await _productServices.CreateProduct(productDto);
            return RedirectToAction(nameof(Index));
        }


        // GET: Products/Edit/5
        // کاربر می‌خواهد محصولی را ویرایش کند فراخوانی می‌شود.
        //ررسی می‌کند که id معتبر باشد 
        //حصول با آن id را از سرویس محصولات می‌گیرد.
        //لیست دسته‌بندی‌ها را می‌گیرد و به ویو می‌فرستد تا کاربر بتواند دسته‌بندی را انتخاب کند.
        //ده‌های محصول را به یک ProductDto تبدیل می‌کند تا به ویو ارسال شود.
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var product = await _productServices.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }


            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ImagePath = product.ImagePath,
                IsAvail = product.IsAvail,
                ShomeHomePage = product.ShomeHomePage
            };

            var categories = await _categoryService.GetCategories();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", productDto.CategoryId);
            return View(productDto);

        }

        // POST: Products/Edit/5
        // کاربر فرم ویرایش را ارسال می‌کند.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ProductDto productDto)
        {
            if (id != productDto.Id)
            {
                return NotFound();
            }
            //اگر همه فیلدها درست باشند و هیچ خطایی وجود نداشته باشد، مقدارش ترو میشه
            if (ModelState.IsValid)
            {
                var categories = await _categoryService.GetCategories();
                ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", productDto.CategoryId);
                return View(productDto);
            }

            try
            {
                await _productServices.UpdateProduct(productDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "خطا در به‌روزرسانی محصول: " + ex.Message);
                var categories = await _categoryService.GetCategories();
                ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", productDto.CategoryId);
                return View(productDto);
            }
        }





        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productServices.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _productServices.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
