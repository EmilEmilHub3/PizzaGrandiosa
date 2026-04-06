//using PizzaGrandiosa.Models;

using PizzaModels.Models;

namespace PizzaGrandiosa.Repositories
{
    public interface IProductRepository
    {
        public Task<Product?> Get(int id);

        public Task<Product?> Add(Product product);

        public Task<IEnumerable<Product>> GetAllProductsAsync();



    }
}
