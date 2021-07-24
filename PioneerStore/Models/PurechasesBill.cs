using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PioneerStore.Models
{
    public class PurechasesBill
    {
        [Required]
        public Purchases_Bills Bill { get; set; }

        [Required]
        public List<Purchases_Bills_Details> Items { get; set; }
    }
}