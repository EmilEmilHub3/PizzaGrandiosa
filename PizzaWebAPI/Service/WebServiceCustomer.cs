using PizzaModels.Models;
using PizzaWebAPI.DTO;
using System.Net.Http.Json;
using System.Text.Json;

namespace PizzaWebAPI.Service
{
    public interface IWebServiceCustomer
    {
        Task<CustomerDTO> Get(int id);
        Task<List<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO?> Add(CustomerDTO customer);
    }

    public class WebServiceCustomer : IWebServiceCustomer
    {
        private readonly IHttpClientFactory _factory;

        public WebServiceCustomer(IHttpClientFactory factory)
        {
            _factory = factory;
        }
        
        public async Task<CustomerDTO?> Add(CustomerDTO customerDTO)
        {
            using HttpClient client = _factory.CreateClient("Default");

            var customer = customerDTO.GetAsCustomer();
            var customerResponse = await client.PostAsJsonAsync<Customer>($"/api/customer/", customer);
            customerResponse.EnsureSuccessStatusCode();

            var newCustomer = await customerResponse.Content.ReadFromJsonAsync<Customer>();

            return new CustomerDTO(newCustomer);
        }

        public async Task<CustomerDTO> Get(int id)
        {
            using HttpClient client = _factory.CreateClient("Default");

            Customer? customer = await client.GetFromJsonAsync<Customer>($"/api/customer/{id}");

            return new CustomerDTO(customer);
        }

        public async Task<List<CustomerDTO>> GetAllCustomersAsync()
        {
            using HttpClient client = _factory.CreateClient("Default");

            List<Customer> customers = await client.GetFromJsonAsync<List<Customer>>($"/api/customer/");

            var customerDTOs = customers?.ConvertAll(x => new CustomerDTO(x));

            return customerDTOs;
        }


    }
}
