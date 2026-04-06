using Microsoft.EntityFrameworkCore;
using PizzaGrandiosa.Persistence;
using PizzaModels.Models;

namespace PizzaGrandiosa.Repositories
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly PizzaDbContext _dbContext;

        public SalesOrderRepository(PizzaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SalesOrder?> Add(SalesOrder salesOrder)
        {
            salesOrder.Date = salesOrder.Date.ToUniversalTime();
            await _dbContext.SalesOrders.AddAsync(salesOrder);
            await _dbContext.SaveChangesAsync();
            return salesOrder;
        }

        public async Task<SalesOrder?> Get(int id)
        {
            var salesOrdersRetrieved = await _dbContext.SalesOrders
                .Include(m => m.CustomerFK)
                .Include(m => m.SalesLines)
                .ThenInclude(salesLines => salesLines.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (salesOrdersRetrieved == null)
                return null;
            return salesOrdersRetrieved;
        }

        public async Task<IEnumerable<SalesOrder>> GetAllSalesOrdersAsync()
        {
            return await _dbContext.SalesOrders
              .Include(m => m.CustomerFK)
              .Include(m => m.SalesLines)
              .ThenInclude(salesLines => salesLines.Product)
              .AsNoTracking()
              .ToListAsync();
        }
    }
}
