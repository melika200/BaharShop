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
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IDatabaseContext _context;
        public ShoppingCartRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public async Task Add(ShoppingCart shoppingcart)
        {
            _context.ShoppingCarts.Add(shoppingcart);
            await _context.SaveChangesAsync();
        }

        public async Task AddBasketItem(ShoppingCartItem item)
        {
            _context.ShoppingCartItems.Add(item);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateBasketItem(ShoppingCartItem item)
        {
            _context.ShoppingCartItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var cart = await GetById(id);
            _context.ShoppingCarts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(ShoppingCart shoppingcart)
        {
            _context.ShoppingCarts.Remove(shoppingcart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBasketItem(ShoppingCartItem item)
        {
            _context.ShoppingCartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public IQueryable<ShoppingCart> GetAll(Expression<Func<ShoppingCart, bool>> where = null)
        {
            var carts = _context.ShoppingCarts.AsQueryable();
            if (where != null)
            {
                carts = carts.Where(where);
            }

            return carts;
        }

        public IQueryable<ShoppingCartItem> GetAllBasketItems(Expression<Func<ShoppingCartItem, bool>> where = null)
        {
            var baskets = _context.ShoppingCartItems.AsQueryable();
            if (where != null)
            {
                baskets = baskets.Where(where);
            }

            return baskets;
        }

        public async Task<ShoppingCart> GetById(int id)
        {
            return await _context.ShoppingCarts.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Update(ShoppingCart shoppingcart)
        {
            _context.ShoppingCarts.Update(shoppingcart);
            await _context.SaveChangesAsync();
        }

    }
}
