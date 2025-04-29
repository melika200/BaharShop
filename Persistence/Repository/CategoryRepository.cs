using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Application.Interface.Context;
using BaharShop.Application.InterfaceRepository;
using BaharShop.Domain;
using Microsoft.EntityFrameworkCore;

namespace BaharShop.Persistence.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDatabaseContext _context;
        public CategoryRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public async Task Add(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var category = await GetById(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetAll(Expression<Func<Category, bool>> where = null)
        {
            if (where != null)
            {
                return await _context.Categories.Where(where).ToListAsync();
            }
            else
            {
                return await _context.Categories.ToListAsync();
            }
        }

        public async Task<Category> GetById(long id)
        {
            return await _context.Categories.Include(a => a.Products).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Update(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
