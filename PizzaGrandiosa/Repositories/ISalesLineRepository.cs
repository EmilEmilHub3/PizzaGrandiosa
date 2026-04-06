//using PizzaGrandiosa.Models;

using PizzaModels.Models;

namespace PizzaGrandiosa.Repositories
{
    public interface ISalesLineRepository
    {
        public Task<SalesLine?> Get(int id);

        public Task<SalesLine?> Add(SalesLine SalesLine);

        public Task<IEnumerable<SalesLine>> GetAllSalesLinesAsync();



    }
}
