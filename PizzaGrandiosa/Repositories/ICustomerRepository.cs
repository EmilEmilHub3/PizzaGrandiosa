//using PizzaGrandiosa.Models;

using PizzaModels.Models;

namespace PizzaGrandiosa.Repositories
{
    public interface ICustomerRepository
    {
        public Task<Customer?> Get(int id);

        public Task<Customer?> Add(Customer customer);

        public Task<IEnumerable<Customer>> GetAllCustomerAsync();



    }
}
