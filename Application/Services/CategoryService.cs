using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Application.InterfaceRepository;
using BaharShop.Domain;

namespace BaharShop.Application.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public async Task<IEnumerable<Category>> GetCategories(Expression<Func<Category, bool>> where = null)
        {
            return await _categoryRepository.GetAll(where);
        }




        public async Task CreateCategory(Category category)
        {
            await _categoryRepository.Add(category);
        }


        public async Task<Category> GetCategoryById(long id)
        {
            return await _categoryRepository.GetById(id);
        }


        public async Task UpdateCategory(Category category)
        {
            await _categoryRepository.Update(category);
        }


        public async Task DeleteCategory(long id)
        {
            await _categoryRepository.Delete(id);
        }


    }
}
