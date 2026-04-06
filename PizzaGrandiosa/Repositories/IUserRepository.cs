//using PizzaGrandiosa.Models;

using PizzaModels.Models;

namespace PizzaGrandiosa.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> Get(int id);

        public Task<User?> Add(User user);

        public Task<User?> Update(User user);
         

        public Task<IEnumerable<User>> GetAllUsersAsync();



    }
}
