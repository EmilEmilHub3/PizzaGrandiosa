using Microsoft.EntityFrameworkCore;
using PizzaGrandiosa.Persistence;
using PizzaModels.Models;

namespace PizzaGrandiosa.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PizzaDbContext _dbContext;

        public ProductRepository(PizzaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> Add(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> Get(int id)
        {
            var productsRetrieved = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (productsRetrieved == null)
                return null;
            return productsRetrieved;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Select(u => new Product
                {
                    Id = u.Id,
                    Description = u.Description,
                    Price = u.Price,
                    Type = u.Type
                })
                .ToListAsync();
        }
    }
}
