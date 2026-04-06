using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace PizzaModels.Models
{
    [Table("SalesOrder")]
    public class SalesOrder : EntityBase
    {
        public int CustomerId { get; set; }

        [JsonIgnore]
        [ForeignKey("CustomerId")]
        public Customer CustomerFK { get; set; }

        public string? OrderType { get; set; }

        public bool IsAccepted { get; set; } = false;

        public bool IsPosted { get; set; } = false;

        public DateTime Date { get; set; }

        public ICollection<SalesLine> SalesLines { get; set; } = new List<SalesLine>();
    }
}
