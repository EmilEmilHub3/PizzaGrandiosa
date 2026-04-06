using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace PizzaModels.Models
{
    [Table("Customer")]
    public class Customer : EntityBase
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
    }
}
