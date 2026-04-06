using PizzaModels.Models;
using PizzaWebAPI.DTO;
using System.Net.Http.Json;
using System.Text.Json;

namespace PizzaWebAPI.Service
{
    public interface IWebServiceUser
    {
        Task<UserDTO> Get(int id);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO?> Update(UserDTO user);
        Task<UserDTO?> Add(UserDTO user);
    }

    public class WebServiceUser : IWebServiceUser
    {
        private readonly IHttpClientFactory _factory;

        public WebServiceUser(IHttpClientFactory factory)
        {
            _factory = factory;
        }
        
        public async Task<UserDTO?> Add(UserDTO userDTO)
        {
            using HttpClient client = _factory.CreateClient("Default");

            var user = userDTO.GetAsUser();
            var userResponse = await client.PostAsJsonAsync<User>($"/api/users/", user);
            userResponse.EnsureSuccessStatusCode();

            var newUser = await userResponse.Content.ReadFromJsonAsync<User>();

            return new UserDTO(newUser);
        }

        public async Task<UserDTO> Get(int id)
        {
            using HttpClient client = _factory.CreateClient("Default");

            User? user = await client.GetFromJsonAsync<User>($"/api/users/{id}");

            return new UserDTO(user);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            using HttpClient client = _factory.CreateClient("Default");

            List<User> users = await client.GetFromJsonAsync<List<User>>($"/api/users/");

            var userDTOs = users?.ConvertAll(x => new UserDTO(x));

            return userDTOs;
        }

        public async Task<UserDTO?> Update(UserDTO userDTO)
        {
            using HttpClient client = _factory.CreateClient("Default");

            var user = userDTO.GetAsUser();
            var userResponse = await client.PutAsJsonAsync<User>($"/api/users/", user);
            userResponse.EnsureSuccessStatusCode();

            var updatedUser = await userResponse.Content.ReadFromJsonAsync<User>();

            return new UserDTO(updatedUser);
        }
    }
}
