using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Domain;

namespace BaharShop.Application.InterfaceRepository
{
    public interface ICategoryRepository
    {
        Task<Category> GetById(long id);
        Task<IEnumerable<Category>> GetAll(Expression<Func<Category, bool>> where = null);
        Task Add(Category category);
        Task Update(Category category);
        Task Delete(long id);
    }
}
