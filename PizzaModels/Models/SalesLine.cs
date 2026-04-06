using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace PizzaModels.Models
{
    [Table("SalesLine")]
    //[PrimaryKey(nameof(Id), nameof(SalesLineNo))]
    public class SalesLine : EntityBase
    {
        //[Key, Column(Order = 1)]
        //public int SalesLineNo { get; set; }

        public int SalesOrderId { get; set; }

        [JsonIgnore]
        [ForeignKey("SalesOrderId")]
        public SalesOrder SalesOrderFK { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
    