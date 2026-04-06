using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
//using PizzaGrandiosa.Models;
using PizzaGrandiosa.Persistence;
using PizzaModels.Models;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PizzaGrandiosa.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly PizzaDbContext _dbContext;

        public CustomerRepository(PizzaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer?> Get(int id)
        {
            Console.WriteLine($"get customer by id {id} from db");
            var customerRetrieved = await _dbContext.Customers
                .Include(m => m.SalesOrders)
                //.ThenInclude(SalesOrdersa=> SalesOrdersa.SalesLines)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerRetrieved == null)
                return null;
            return customerRetrieved;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomerAsync()
        {
            return await _dbContext.Customers
                .Include(m => m.SalesOrders)
                //.ThenInclude(SalesOrdersa => SalesOrdersa.SalesLines)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<Customer?> Add(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();            

            //If this was a front facing API, use a DTO version of the User (UserDTO)
            return customer;

        }

    }
}
