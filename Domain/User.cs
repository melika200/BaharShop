using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Domain.Common;

namespace BaharShop.Domain
{
    public class User : BaseEntity
    {
        public long Id { get; set; } 
        public string Username { get; set; } 
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; } 
        public string FirstName { get; set; } 
        public string LastName { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now; 
        public bool IsActive { get; set; } = true;

        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
