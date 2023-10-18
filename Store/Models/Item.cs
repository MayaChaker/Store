using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public int quantity { get; set; }
        
    }
}