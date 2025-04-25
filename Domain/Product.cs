using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Domain.Common;

namespace BaharShop.Domain
{
    public class Product:BaseEntity
    {
        public string Name { get; set; } 
        public string Img { get; set; }
        public decimal Price { get; set; } 
        public string Description { get; set; } 
        public int Stock { get; set; } 
        public long CategoryId { get; set; } 
        public Category Category { get; set; }
        public string ImagePath { get; set; }
        public bool IsAvail { get; set; }
        public bool ShomeHomePage { get; set; } 
        public double AverageRating { get; set; } 
        public int TotalRatings { get; set; } 
    }
}
