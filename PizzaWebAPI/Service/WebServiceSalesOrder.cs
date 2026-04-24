using PizzaModels.Models;
using PizzaWebAPI.DTO;
using System.Net.Http.Json;

namespace PizzaWebAPI.Service
{
    public interface IWebServiceSalesOrder
    {
        Task<SalesOrderDTO?> Get(int id);
        Task<List<SalesOrderDTO>> GetAllSalesOrdersAsync();
        Task<SalesOrderDTO?> Add(SalesOrderDTO salesorder);
        Task<object?> Accept(int id);
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

            var response = await client.PostAsJsonAsync(
                "/api/salesorder/",
                salesorder);

            response.EnsureSuccessStatusCode();

            var newSalesOrder =
                await response.Content.ReadFromJsonAsync<SalesOrder>();

            return newSalesOrder is null
                ? null
                : new SalesOrderDTO(newSalesOrder);
        }

        public async Task<SalesOrderDTO?> Get(int id)
        {
            using HttpClient client = _factory.CreateClient("Default");

            var response = await client.GetAsync($"/api/salesorder/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var salesorder =
                await response.Content.ReadFromJsonAsync<SalesOrder>();

            return salesorder is null
                ? null
                : new SalesOrderDTO(salesorder);
        }

        public async Task<List<SalesOrderDTO>> GetAllSalesOrdersAsync()
        {
            using HttpClient client = _factory.CreateClient("Default");

            var salesorders =
                await client.GetFromJsonAsync<List<SalesOrder>>(
                    "/api/salesorder/");

            return salesorders?
                .ConvertAll(x => new SalesOrderDTO(x))
                ?? new List<SalesOrderDTO>();
        }

        public async Task<object?> Accept(int id)
        {
            using HttpClient client = _factory.CreateClient("Default");

            var response = await client.PutAsync(
                $"/api/salesorder/{id}/accept",
                null);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<object>();
        }
    }
}