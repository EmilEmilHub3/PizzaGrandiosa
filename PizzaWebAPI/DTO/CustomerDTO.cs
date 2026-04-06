using PizzaModels.Models;

namespace PizzaWebAPI.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }

        public ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();



        public CustomerDTO()
        { }

        public CustomerDTO(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Age = customer.Age;
            SalesOrders = customer.SalesOrders;
        }

        public Customer GetAsCustomer()
        {
            return new Customer
            {
                Id = Id,
                Name = Name,
                Age = Age,
                SalesOrders = SalesOrders
            };
        }
    }
}
