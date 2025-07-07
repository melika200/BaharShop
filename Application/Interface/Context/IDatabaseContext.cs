using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Domain;
using Microsoft.EntityFrameworkCore;

namespace BaharShop.Application.Interface.Context
{
    public interface IDatabaseContext
    {
        // فقط داخل همان کلاس قابل دسترسی هستند و از بیرون کلاس قابل استفاده نیستند.
        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<User> Users { get; set; }

        DbSet<ShoppingCart> ShoppingCarts { get; set; }
        DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        //مثل قبلی، ولی شما تصمیم می‌گیرید بعد از ثبت، تغییرات را در ذهن خود پاک کنید یا نگه دارید.
        int SaveChanges(bool acceptAllChangesOnSuccess);
        //شما می‌روید دفترچه تغییرات را ثبت می‌کنید و منتظر می‌مانید تا تمام شود.
        int SaveChanges();
        //مثل قبلی، با امکان اینکه بعد از ثبت تغییرات را در ذهن پاک کنید یا نگه دارید و همچنین بتوانید عملیات ثبت را لغو کنید.
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
        //دستیار می‌رود دفترچه تغییرات را ثبت می‌کند و شما می‌توانید در این حین کارهای دیگر انجام دهید.
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        //CancellationToken: به شما اجازه می‌دهد عملیات ثبت را وسط کار لغو کنید.
    }
}
