using System;

namespace PizzaGrandiosa.Contracts
{
    public class SalesOrderStatusMessage
    {
        public int SalesOrderId { get; set; }
        public int CustomerId { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsPosted { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime UpdatedAtUtc { get; set; }
    }
}