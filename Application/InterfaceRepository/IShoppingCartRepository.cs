using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Domain;

namespace BaharShop.Application.InterfaceRepository
{
    public interface IShoppingCartRepository
    {
        //همه‌ی سبدهای خرید
        IQueryable<ShoppingCart> GetAll(Expression<Func<ShoppingCart, bool>> where = null);
        Task<ShoppingCart> GetById(int id);

        Task Add(ShoppingCart shoppingcart);
        Task Update(ShoppingCart shoppingcart);
        Task Delete(int id);
        Task Delete(ShoppingCart shoppingcart);
        // همه آیتم‌های داخل سبد خرید
        IQueryable<ShoppingCartItem> GetAllBasketItems(Expression<Func<ShoppingCartItem, bool>> where = null);
        Task AddBasketItem(ShoppingCartItem item);
        Task DeleteBasketItem(ShoppingCartItem item);
        Task UpdateBasketItem(ShoppingCartItem item);

    }
}
