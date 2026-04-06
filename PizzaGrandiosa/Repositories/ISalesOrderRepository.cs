using PizzaModels.Models;

namespace PizzaGrandiosa.Repositories
{
    public interface ISalesOrderRepository
    {
        Task<SalesOrder?> Get(int id);

        Task<SalesOrder?> Add(SalesOrder salesOrder);

        Task<IEnumerable<SalesOrder>> GetAllSalesOrdersAsync();
    }
}