using PizzaWebAPI.DTO;
using PizzaWebAPI.Service;

namespace PizzaWebAPI.Endpoints
{
    public static class CustomerEndpoint
    {
        public static IEndpointRouteBuilder MapCustomerEndpoint(this IEndpointRouteBuilder app) //WebApplication
        {
            var customerApi = app.MapGroup("/api/customer").WithTags("customer");

            customerApi.MapGet("/{id}", async (int id, IWebServiceCustomer ws) =>
            {
                Console.WriteLine($"Get customer by id invoked");   

                var customer = await ws.Get(id);
                return TypedResults.Ok(customer);
            });

            customerApi.MapGet("/", async (IWebServiceCustomer ws) =>
            {
                Console.WriteLine($"Get all customers invoked");

                var customers = await ws.GetAllCustomersAsync();
                return TypedResults.Ok(customers);
            });


            customerApi.MapPost("/", static async (CustomerDTO customerDTO, IWebServiceCustomer ws) => {

                Console.WriteLine($"Post customer invoked");

                CustomerDTO? newCustomer = await ws.Add(customerDTO);
                return TypedResults.Created($"/api/customer/{newCustomer?.Id}", newCustomer);
            });
            return app;
        }
    }
}
