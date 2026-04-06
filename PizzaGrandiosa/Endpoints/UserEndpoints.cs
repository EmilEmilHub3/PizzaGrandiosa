//using PizzaGrandiosa.Models;
using PizzaGrandiosa.Repositories;
using PizzaModels.Models;

namespace PizzaGrandiosa.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
            var userApi = routes.MapGroup("/api/users").WithTags("users");

            userApi.MapGet("/{id}", async (IUserRepository repo, int id) =>
            {
                Console.WriteLine($"Get user by id invoked");

                var user = await repo.Get(id);
                return TypedResults.Ok(user);
            });

            userApi.MapGet("/", async (IUserRepository repo) =>
            {
                Console.WriteLine($"Get all users invoked");

                var users = await repo.GetAllUsersAsync();
                return TypedResults.Ok(users);
            });


     

            userApi.MapPost("/", async (User user, IUserRepository ur) => {

                Console.WriteLine($"Post user invoked");

                User? newUser = await ur.Add(user);
                //return newUser;
                return TypedResults.Created($"/api/users/{newUser?.Id}", newUser);
            });

            userApi.MapPut("/", async (User user, IUserRepository ur) => {

                Console.WriteLine($"Put user invoked");

                User? upUser = await ur.Update(user);
                return TypedResults.Created($"/api/users/{upUser?.Id}", upUser);
            });

        }
    }
}
