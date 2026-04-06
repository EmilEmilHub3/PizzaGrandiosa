using PizzaWebAPI.DTO;
using PizzaWebAPI.Service;

namespace PizzaWebAPI.Endpoints
{
    public static class UserEndpoint
    {
        public static IEndpointRouteBuilder MapUserEndpoint(this IEndpointRouteBuilder app) 
        {
            var userApi = app.MapGroup("/api/users").WithTags("users");

            userApi.MapGet("/{id}", async (int id, IWebServiceUser ws ) =>
            {
                Console.WriteLine($"Get user by id invoked");

                var user = await ws.Get(id);
                return TypedResults.Ok(user);
            });

            userApi.MapGet("/", async (IWebServiceUser ws) =>
            {
                Console.WriteLine($"Get all users invoked");

                var users = await ws.GetAllUsersAsync();
                return TypedResults.Ok(users);
            });


            userApi.MapPost("/", static async (UserDTO user, IWebServiceUser ws) => {

                Console.WriteLine($"Post user invoked");

                UserDTO? newUser = await ws.Add(user);
                //return newUser;
                return TypedResults.Created($"/api/users/{newUser?.Id}", newUser);
            });

            userApi.MapPut("/", async (UserDTO user, IWebServiceUser ws) => {

                Console.WriteLine($"Put user invoked");

                UserDTO? upUser = await ws.Update(user);
                return TypedResults.Created($"/api/users/{upUser?.Id}", upUser);
            });


            return app;
        }


        /// <summary>
        /// Test GUI parameter:
        /// 3fa85f64-5717-4562-b3fc-2c963f66bfc9
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ws"></param>
        /// <returns></returns>
        //private static async Task<UserDTO?> Get(Guid id, IWebService ws)
        //{
        //    Console.WriteLine($"id: {id}");
        //    UserDTO? user = await ws.Get(id);
        //    return user;
        //}

        //private static async Task<UserDTO?> Add(UserDTO user, IWebService ws)
        //{


        //    UserDTO? newUser = null;// await ur.Add(user);

        //    //Console.WriteLine("Post is user null: " + (newUser == null));
        //    //Console.WriteLine("Post id: " + newUser?.Id);

        //    return user;


        //}
    }
}
