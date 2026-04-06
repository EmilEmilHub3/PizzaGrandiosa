using Microsoft.EntityFrameworkCore;
using PizzaGrandiosa.Persistence;
using PizzaModels.Models;

namespace PizzaGrandiosa.Repositories
{
    public class SalesLineRepository : ISalesLineRepository
    {
        private readonly PizzaDbContext _dbContext;

        public SalesLineRepository(PizzaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SalesLine?> Add(SalesLine SalesLine)
        {
            await _dbContext.SalesLines.AddAsync(SalesLine);
            await _dbContext.SaveChangesAsync();

            return SalesLine;
        }

        public async Task<SalesLine?> Get(int id)
        {
            var SalesLinesRetrieved = await _dbContext.SalesLines.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (SalesLinesRetrieved == null)
                return null;
            return SalesLinesRetrieved;
        }

        public async Task<IEnumerable<SalesLine>> GetAllSalesLinesAsync()
        {
            return await _dbContext.SalesLines
                .Include(m => m.SalesOrderFK)
                .Select(u => new SalesLine
                {
                    Id = u.Id,
                    Price = u.Price,
                    Product = u.Product,
                    Quantity = u.Quantity,
                    SalesOrderId = u.SalesOrderId,
                    SalesOrderFK = u.SalesOrderFK
              
                })
                .ToListAsync();
        }
    }
}
