using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Domain.Enum;

namespace BaharShop.Application.Services.Dto
{
    public class AdminOrderDto
    {
        public long Id { get; set; }

        public DateTime Payed { get; set; }

        public long UserId { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public Status Status { get; set; }
        public string UserName { get; set; }
        public List<string> Items { get; set; }
    }
}
