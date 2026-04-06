using PizzaModels.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PizzaWebAPI.DTO
{
    public class SalesOrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public string? OrderType { get; set; }

        public bool IsAccepted { get; set; } = false;

        public bool IsPosted { get; set; } = false;

        public DateTime Date { get; set; }

        public ICollection<SalesLine> SalesLines { get; set; } = new List<SalesLine>();

        public SalesOrderDTO()
        { }

        public SalesOrderDTO(SalesOrder salesOrder)
        {
            Id = salesOrder.Id;
            CustomerId = salesOrder.CustomerId;
            OrderType = salesOrder.OrderType;
            IsAccepted = salesOrder.IsAccepted;
            Date = salesOrder.Date;
            SalesLines = salesOrder.SalesLines;
        
        }

        public SalesOrder GetAsSalesOrder()
        {
            return new SalesOrder
            {
                Id = Id,
                CustomerId = CustomerId,
                OrderType = OrderType,
                IsAccepted = IsAccepted,
                Date = Date,
                SalesLines = SalesLines
            };
        }
    }
}
