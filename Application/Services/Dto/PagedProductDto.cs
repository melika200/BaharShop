﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaharShop.Application.Services.Dto
{
    public class PagedProductDto
    {

        public int TotalPage { get; set; }
        public int Page { get; set; }
        public List<ProductDto> Items { get; set; }

    }
}
