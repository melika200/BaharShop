using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Application.Interface.Context;
using BaharShop.Domain;
using Microsoft.EntityFrameworkCore;

namespace BaharShop.Persistence.Context
{
    // برای مدیریت ارتباط با دیتابیس
    public class StoreDbContext :DbContext, IDatabaseContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }
        //هر کدی که به این کلاس دسترسی داشته باشد، می‌تواند به این مجموعه‌ها دسترسی داشته باشد.
        //عمولاً در کلاس‌های DbContext این حالت استفاده می‌شود چون می‌خواهیم از بیرون به داده‌ها دسترسی داشته باشیم.

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyQueryFilter(modelBuilder);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.User)
                .WithMany(c => c.ShoppingCarts)
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.ShoppingCart)
                .WithMany(sc => sc.Items)
                .HasForeignKey(sci => sci.ShoppingCartId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ShoppingCartItem>()
                .Property(sci => sci.Price)
                .HasColumnType("decimal(18,2)");
        }

        private void ApplyQueryFilter(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsRemoved);
            modelBuilder.Entity<ShoppingCart>().HasQueryFilter(sc => !sc.IsRemoved);
            modelBuilder.Entity<ShoppingCartItem>().HasQueryFilter(sci => !sci.IsRemoved);

        }
        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
