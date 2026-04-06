using Microsoft.EntityFrameworkCore;
using PizzaGrandiosa.Persistence;
using PizzaModels.Models;

namespace PizzaGrandiosa.Repositories
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly PizzaDbContext _context;

        public SalesOrderRepository(PizzaDbContext context)
        {
            _context = context;
        }

        public async Task<SalesOrder?> Add(SalesOrder salesOrder)
        {
            _context.SalesOrders.Add(salesOrder);
            await _context.SaveChangesAsync();

            return await _context.SalesOrders
                .Include(o => o.SalesLines)
                .ThenInclude(sl => sl.Product)
                .FirstOrDefaultAsync(o => o.Id == salesOrder.Id);
        }

        public async Task<SalesOrder?> Get(int id)
        {
            return await _context.SalesOrders
                .Include(o => o.SalesLines)
                .ThenInclude(sl => sl.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<SalesOrder>> GetAllSalesOrdersAsync()
        {
            return await _context.SalesOrders
                .Include(o => o.SalesLines)
                .ThenInclude(sl => sl.Product)
                .ToListAsync();
        }
    }
}