using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Domain.Common;
using BaharShop.Domain.Enum;

namespace BaharShop.Domain
{
    public class ShoppingCart : BaseEntity
    {
        public long Id { get; set; }
        public DateTime Payed { get; set; }
        public long UserId { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public Status Status { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<ShoppingCartItem> Items { get; set; }
    }
}
