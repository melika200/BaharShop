﻿using System;
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
    public class ProductRepository : IProductRepository
    {
        private readonly IDatabaseContext _context;

        public ProductRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public async Task Add(Product product)
        {

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var product = await GetById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAll(Expression<Func<Product, bool>> where = null)
        {
            IQueryable<Product> products = _context.Products;

            if (where != null)
            {
                products = products.Where(where);
            }

            return await products.ToListAsync();
        }
        //محصولات همراه با اطلاعات دسته‌بندی‌شان
        public async Task<IEnumerable<Product>> GetAllWithCategory(Expression<Func<Product, bool>> where = null)
        {
            IQueryable<Product> products = _context.Products.Include(p => p.Category);

            if (where != null)
            {
                products = products.Where(where);
            }

            return await products.ToListAsync();
        }


        public async Task<Product> GetById(long id)
        {
            return await _context.Products
               .Include(p => p.Category) 
               .FirstOrDefaultAsync(p => p.Id == id);
        }
        //فقط محصولاتی که در دسته‌بندی مشخص شده 
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(long categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)  
                .Include(p => p.Category)                 
                .ToListAsync();                      
        }


        public async Task Update(Product product)
        {

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRating(long productId, int rating)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product != null)
            {
                product.TotalRatings += 1;
                product.AverageRating = ((product.AverageRating * (product.TotalRatings - 1)) + rating) / product.TotalRatings;
                await _context.SaveChangesAsync();
            }
        }
    }
}
