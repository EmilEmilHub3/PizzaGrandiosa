//using PizzaGrandiosa.Models;
using PizzaGrandiosa.Repositories;
using PizzaModels.Models;

namespace PizzaGrandiosa.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes)
        {
            var CustomerApi = routes.MapGroup("/api/customer").WithTags("customer");

            CustomerApi.MapGet("/{id}", async ( int id, ICustomerRepository repo) =>
            {
                Console.WriteLine($"Get customer by id invoked");

                var customer = await repo.Get(id);
                return TypedResults.Ok(customer);
            });

            CustomerApi.MapGet("/", async (ICustomerRepository repo) =>
            {
                Console.WriteLine($"Get all customers invoked");

                var customer = await repo.GetAllCustomerAsync();
                return TypedResults.Ok(customer);
            });


            CustomerApi.MapPost("/", async (Customer customer, ICustomerRepository cr) => {

                Console.WriteLine($"Post customer invoked");

                Customer? newCustomer = await cr.Add(customer);
                return TypedResults.Created($"/api/customer/{newCustomer?.Id}", newCustomer);
            });

        }
    }
}
