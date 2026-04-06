using PizzaModels.Models;
using PizzaWebAPI.DTO;
using System.Net.Http.Json;
using System.Text.Json;

namespace PizzaWebAPI.Service
{
    public interface IWebServiceSalesLine
    {
        Task<SalesLineDTO> Get(int id);
        Task<List<SalesLineDTO>> GetAllSalesLinesAsync();
        Task<SalesLineDTO?> Add(SalesLineDTO salseline);
    }

    public class WebServiceSalesLine : IWebServiceSalesLine
    {
        private readonly IHttpClientFactory _factory;

        public WebServiceSalesLine(IHttpClientFactory factory)
        {
            _factory = factory;
        }
        
        public async Task<SalesLineDTO?> Add(SalesLineDTO salselineDTO)
        {
            using HttpClient client = _factory.CreateClient("Default");

            var salseline = salselineDTO.GetAsSalesLine();
            var salselineResponse = await client.PostAsJsonAsync<SalesLine>($"/api/saleslines/", salseline);
            salselineResponse.EnsureSuccessStatusCode();

            var newSalesLine = await salselineResponse.Content.ReadFromJsonAsync<SalesLine>();

            return new SalesLineDTO(newSalesLine);
        }

        public async Task<SalesLineDTO> Get(int id)
        {
            using HttpClient client = _factory.CreateClient("Default");

            SalesLine? salesline = await client.GetFromJsonAsync<SalesLine>($"/api/saleslines/");

            return new SalesLineDTO(salesline);
        }

        public async Task<List<SalesLineDTO>> GetAllSalesLinesAsync()
        {
            using HttpClient client = _factory.CreateClient("Default");

            List<SalesLine> saleslines = await client.GetFromJsonAsync<List<SalesLine>>($"/api/saleslines/");

            var saleslineDTOs = saleslines?.ConvertAll(x => new SalesLineDTO(x));

            return saleslineDTOs;
        }


    }
}
