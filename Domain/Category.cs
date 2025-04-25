using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Domain.Common;

namespace BaharShop.Domain
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } 
        public ICollection<Product> Products { get; set; }
    }
}
