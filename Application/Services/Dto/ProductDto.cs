using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BaharShop.Application.Services.Dto
{
    public class ProductDto
    {
        public long Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } 
        public int Stock { get; set; } 
        public long CategoryId { get; set; }
        public string ImagePath { get; set; } 
        public bool ShomeHomePage { get; set; }
        public bool IsAvail { get; set; }
        public IFormFile? Img { get; set; } 
        public string ImgName { get; set; } 
    }
}
