using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Domain.Common;

namespace BaharShop.Domain
{

    public class ShoppingCartItem : BaseEntity
    {

        public long ShoppingCartId { get; set; } 
        public ShoppingCart ShoppingCart { get; set; } 
        public long ProductId { get; set; } 
        public Product Product { get; set; }
        public int Quantity { get; set; } 
        public decimal Price { get; set; }

    }
}
