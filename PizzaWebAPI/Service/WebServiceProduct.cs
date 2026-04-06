using PizzaModels.Models;
using PizzaWebAPI.DTO;
using System.Net.Http.Json;
using System.Text.Json;

namespace PizzaWebAPI.Service
{
    public interface IWebServiceProduct
    {
        Task<ProductDTO> Get(int id);
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO?> Add(ProductDTO product);
    }

    public class WebServiceProduct : IWebServiceProduct
    {
        private readonly IHttpClientFactory _factory;

        public WebServiceProduct(IHttpClientFactory factory)
        {
            _factory = factory;
        }
        
        public async Task<ProductDTO?> Add(ProductDTO productDTO)
        {
            using HttpClient client = _factory.CreateClient("Default");

            var product = productDTO.GetAsProduct();
            var productResponse = await client.PostAsJsonAsync<Product>($"/api/product/", product);
            productResponse.EnsureSuccessStatusCode();

            var newProduct = await productResponse.Content.ReadFromJsonAsync<Product>();

            return new ProductDTO(newProduct);
        }

        public async Task<ProductDTO> Get(int id)
        {
            using HttpClient client = _factory.CreateClient("Default");

            Product? product = await client.GetFromJsonAsync<Product>($"/api/product/{id}");

            return new ProductDTO(product);
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            using HttpClient client = _factory.CreateClient("Default");

            List<Product> products = await client.GetFromJsonAsync<List<Product>>($"/api/product/");

            var productDTOs = products?.ConvertAll(x => new ProductDTO(x));

            return productDTOs;
        }


    }
}
