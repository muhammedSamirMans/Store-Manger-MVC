using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PioneerStore.Models
{
    public class SalesBills
    {
        public Sales_Bills Bill{ get; set; }
        public List<Sales_Bills_Details> Items { get; set; }
    }
}