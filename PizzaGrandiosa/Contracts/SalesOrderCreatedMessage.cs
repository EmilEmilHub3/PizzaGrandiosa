using System;
using System.Collections.Generic;

namespace PizzaGrandiosa.Contracts
{
    public class SalesOrderCreatedMessage
    {
        public int SalesOrderId { get; set; }
        public int CustomerId { get; set; }
        public string? OrderType { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsPosted { get; set; }
        public DateTime Date { get; set; }
        public List<SalesLineMessage> SalesLines { get; set; } = new();
    }

    public class SalesLineMessage
    {
        public int Id { get; set; }
        public int SalesOrderId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int ProductId { get; set; }
        public ProductMessage? Product { get; set; }
    }

    public class ProductMessage
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
    }
}