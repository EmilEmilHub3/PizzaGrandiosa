using PizzaWebAPI.DTO;
using PizzaWebAPI.Service;

namespace PizzaWebAPI.Endpoints
{
    public static class SalesLineEndpoint
    {
        public static IEndpointRouteBuilder MapSalesLineEndpoint(this IEndpointRouteBuilder app) //WebApplication
        {
            var userApi = app.MapGroup("/api/salesLines").WithTags("salesLines");

            userApi.MapGet("/{id}", async (int id, IWebServiceSalesLine ws) =>
            {
                Console.WriteLine($"Get salesLine by id invoked");

                var salesLine = await ws.Get(id);
                return TypedResults.Ok(salesLine);
            });

            userApi.MapGet("/", async (IWebServiceSalesLine ws) =>
            {
                Console.WriteLine($"Get all salesLines invoked");

                var salesLines = await ws.GetAllSalesLinesAsync();
                return TypedResults.Ok(salesLines);
            });


            userApi.MapPost("/", static async (SalesLineDTO salesLine, IWebServiceSalesLine ws) => {

                Console.WriteLine($"Post salesLine invoked");

                SalesLineDTO? newSalesLine = await ws.Add(salesLine);
                return TypedResults.Created($"/api/salesLines/{newSalesLine?.Id}", newSalesLine);
            });

            return app;
        }
    }
}
