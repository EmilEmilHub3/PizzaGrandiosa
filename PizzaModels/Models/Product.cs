using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PizzaModels.Models
{
    [Table("Product")]
    public class Product : EntityBase
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
