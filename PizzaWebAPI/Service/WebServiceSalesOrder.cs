using PizzaModels.Models;
using PizzaWebAPI.DTO;
using System.Net.Http.Json;
using System.Text.Json;

namespace PizzaWebAPI.Service
{
    public interface IWebServiceSalesOrder
    {
        Task<SalesOrderDTO> Get(int id);
        Task<List<SalesOrderDTO>> GetAllSalesOrdersAsync();
        Task<SalesOrderDTO?> Add(SalesOrderDTO salesorder);
    }

    public class WebServiceSalesOrder : IWebServiceSalesOrder
    {
        private readonly IHttpClientFactory _factory;

        public WebServiceSalesOrder(IHttpClientFactory factory)
        {
            _factory = factory;
        }
        
        public async Task<SalesOrderDTO?> Add(SalesOrderDTO salesorderDTO)
        {
            using HttpClient client = _factory.CreateClient("Default");

            var salesorder = salesorderDTO.GetAsSalesOrder();
            var salesorderResponse = await client.PostAsJsonAsync<SalesOrder>($"/api/salesorder/", salesorder);
            salesorderResponse.EnsureSuccessStatusCode();

            var newSalesOrder = await salesorderResponse.Content.ReadFromJsonAsync<SalesOrder>();

            return new SalesOrderDTO(newSalesOrder);
        }

        public async Task<SalesOrderDTO> Get(int id)
        {
            using HttpClient client = _factory.CreateClient("Default");

            SalesOrder? salesorder = await client.GetFromJsonAsync<SalesOrder>($"/api/salesorder/");

            return new SalesOrderDTO(salesorder);
        }

        public async Task<List<SalesOrderDTO>> GetAllSalesOrdersAsync()
        {
            using HttpClient client = _factory.CreateClient("Default");

            List<SalesOrder> salesorders = await client.GetFromJsonAsync<List<SalesOrder>>($"/api/salesorder/");

            var salesorderDTOs = salesorders?.ConvertAll(x => new SalesOrderDTO(x));

            return salesorderDTOs;
        }


    }
}
