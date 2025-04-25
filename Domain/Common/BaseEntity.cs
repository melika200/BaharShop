﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaharShop.Domain.Common
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
        public DateTime InsertTime { get; set; } = DateTime.Now;
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public bool IsRemoved { get; set; } = false;
        public DateTime RemoveTime { get; set; } = DateTime.Now;
    }
    public abstract class BaseEntity : BaseEntity<long>
    {
    }
}
