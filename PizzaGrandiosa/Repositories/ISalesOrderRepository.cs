//using PizzaGrandiosa.Models;

using PizzaModels.Models;

namespace PizzaGrandiosa.Repositories
{
    public interface ISalesOrderRepository
    {
        public Task<SalesOrder?> Get(int id);

        public Task<SalesOrder?> Add(SalesOrder salesOrder);

        public Task<IEnumerable<SalesOrder>> GetAllSalesOrdersAsync();



    }
}
