using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PioneerStore.Models
{
    public class ItemProfit
    {
        public Category item { get; set; }
        public decimal profit { get; set; }
    }
}