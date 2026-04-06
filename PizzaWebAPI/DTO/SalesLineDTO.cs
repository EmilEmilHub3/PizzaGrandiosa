using Microsoft.AspNetCore.Mvc.RazorPages;
using PizzaModels.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace PizzaWebAPI.DTO
{
    public class SalesLineDTO
    {
        public int Id { get; set; }

        public int SalesOrderId { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }


        public SalesLineDTO()
        { }

        public SalesLineDTO(SalesLine salesLine)
        {
            Id = salesLine.Id;
            SalesOrderId = salesLine.SalesOrderId;
            Quantity = salesLine.Quantity;
            Price = salesLine.Price;
            ProductId = salesLine.ProductId;

        }

        public SalesLine GetAsSalesLine()
        {
            return new SalesLine
            {
                Id = Id,
                SalesOrderId = SalesOrderId,
                Quantity = Quantity,
                Price = Price,
                ProductId = ProductId
            };
        }
    }
}
