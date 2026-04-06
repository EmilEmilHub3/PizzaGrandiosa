using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
//using PizzaGrandiosa.Models;
using PizzaGrandiosa.Persistence;
using PizzaModels.Models;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PizzaGrandiosa.Repositories
{
    public class UserRepository : IUserRepository
    {
        //private IDatabase _database;
        //private readonly IConnectionMultiplexer _redis;
        private readonly PizzaDbContext _dbContext;

        public UserRepository(PizzaDbContext dbContext)
        {
            //_redis = connection;
            _dbContext = dbContext;

        }

        public async Task<User?> Get(int id)
        {

            var userRetrieved = await _dbContext.Users
                    .Include(m => m.Customer)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == id);
            if (userRetrieved == null)
                return null;

            return userRetrieved;
        
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users
                .Include(m => m.Customer)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<User?> Add(User user)
        {


            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            //If this was a front facing API, use a DTO version of the User (UserDTO)
            return user;

        }

        public async Task<User?> Update(User user)
        {
            Console.WriteLine($"put user: {user.Id}");

            var userToUpdate = await _dbContext.Users.FindAsync(user.Id);
            if (userToUpdate is null) throw new ArgumentNullException($"Invalid User Id.");

            //userToUpdate.CustomerId = user.CustomerId;
            userToUpdate.Update(user);

            _dbContext.Users.Update(userToUpdate);
            int changes = await _dbContext.SaveChangesAsync();

            if( changes == 0) throw new ArgumentNullException($"User Id could not be updated.");

            //await _dbContext.Users.up AddAsync(user);
            //await _dbContext.SaveChangesAsync();

            //If this was a front facing API, use a DTO version of the User (UserDTO)
            return user;

  
    }
    }
}
