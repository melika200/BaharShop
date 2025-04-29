using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaharShop.Domain;
using BaharShop.Persistence.Context;
using BaharShop.Application.Services;

namespace AdminSite.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategories();
            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryById(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,IsRemoved")] Category category)
        {


            if (!ModelState.IsValid)
            {
                category.InsertTime = DateTime.Now;
                category.UpdateTime = DateTime.Now;
                await _categoryService.CreateCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryById(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name,Id,IsRemoved")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    category.UpdateTime = DateTime.Now;
                    await _categoryService.UpdateCategory(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryById(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _categoryService.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoryExists(long id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return category != null;
        }
    }
}
