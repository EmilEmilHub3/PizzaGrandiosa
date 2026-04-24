using PizzaModels.Models;
using PizzaWebAPI.DTO;
using System.Net.Http.Json;

namespace PizzaWebAPI.Service
{
    public interface IWebServiceSalesLine
    {
        Task<SalesLineDTO?> Get(int id);
        Task<List<SalesLineDTO>> GetAllSalesLinesAsync();
        Task<SalesLineDTO?> Add(SalesLineDTO salesline);
    }

    public class WebServiceSalesLine : IWebServiceSalesLine
    {
        private readonly IHttpClientFactory _factory;

        public WebServiceSalesLine(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<SalesLineDTO?> Add(SalesLineDTO saleslineDTO)
        {
            using HttpClient client = _factory.CreateClient("Default");

            var salesline = saleslineDTO.GetAsSalesLine();

            var response = await client.PostAsJsonAsync(
                "/api/saleslines/",
                salesline);

            response.EnsureSuccessStatusCode();

            var newSalesLine =
                await response.Content.ReadFromJsonAsync<SalesLine>();

            return newSalesLine is null
                ? null
                : new SalesLineDTO(newSalesLine);
        }

        public async Task<SalesLineDTO?> Get(int id)
        {
            using HttpClient client = _factory.CreateClient("Default");

            var response = await client.GetAsync($"/api/saleslines/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var salesline =
                await response.Content.ReadFromJsonAsync<SalesLine>();

            return salesline is null
                ? null
                : new SalesLineDTO(salesline);
        }

        public async Task<List<SalesLineDTO>> GetAllSalesLinesAsync()
        {
            using HttpClient client = _factory.CreateClient("Default");

            var saleslines =
                await client.GetFromJsonAsync<List<SalesLine>>("/api/saleslines/");

            return saleslines?
                .ConvertAll(x => new SalesLineDTO(x))
                ?? new List<SalesLineDTO>();
        }
    }
}