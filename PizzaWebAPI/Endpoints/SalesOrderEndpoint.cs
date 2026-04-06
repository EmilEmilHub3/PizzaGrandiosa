using PizzaModels.Models;
using PizzaWebAPI.DTO;
using PizzaWebAPI.Service;

namespace PizzaWebAPI.Endpoints
{
    public static class SalesOrderEndpoint
    {
        public static IEndpointRouteBuilder MapSalesOrderEndpoint(this IEndpointRouteBuilder app) //WebApplication
        {
            var userApi = app.MapGroup("/api/salesorder").WithTags("salesorder");

            userApi.MapGet("/{id}", async (int id, IWebServiceSalesOrder ws) =>
            {
                Console.WriteLine($"Get salesorder by id invoked");

                var salesorder = await ws.Get(id);
                return TypedResults.Ok(salesorder);
            });

            userApi.MapGet("/", async (IWebServiceSalesOrder ws) =>
            {
                Console.WriteLine($"Get all salesorders invoked");

                var salesorders = await ws.GetAllSalesOrdersAsync();
                return TypedResults.Ok(salesorders);
            });


            userApi.MapPost("/", static async (SalesOrderDTO salesOrder, IWebServiceSalesOrder ws) => {

                Console.WriteLine($"Post user invoked");

                SalesOrderDTO? newSalesOrder = await ws.Add(salesOrder);
                //return newUser;
                return TypedResults.Created($"/api/salesorder/{newSalesOrder?.Id}", newSalesOrder);
            });

  
            return app;
        }
    }
}
