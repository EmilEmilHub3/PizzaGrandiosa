using PizzaModels.Models;
using System.Xml.Linq;

namespace PizzaWebAPI.DTO
{
    public class ProductDTO
    {
        public int Id{ get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }


        public ProductDTO()
        { }

        public ProductDTO(Product product)
        {
            Id = product.Id;
            Type = product.Type;
            Description = product.Description;
            Price = product.Price;
        }

        public Product GetAsProduct()
        {
            return new Product
            {
                Id = Id,
                Type = Type,
                Description = Description,
                Price = Price
            };
        }
    }
}
