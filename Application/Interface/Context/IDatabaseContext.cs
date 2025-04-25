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
        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories { get; set; }
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
