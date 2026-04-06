//using PizzaGrandiosa.Models;
using PizzaGrandiosa.Repositories;
using PizzaModels.Models;

namespace PizzaGrandiosa.Endpoints
{
    public static class SalesLineEndpoints
    {
        public static void MapSalesLilneEndpoints(this IEndpointRouteBuilder routes)
        {
            var salesLineApi = routes.MapGroup("/api/saleslines").WithTags("saleslines");

            salesLineApi.MapGet("/{id}", async (ISalesLineRepository repo, int id) =>
            {
                Console.WriteLine($"Get SalesLine by id invoked");

                var salesLine = await repo.Get(id);
                return TypedResults.Ok(salesLine);
            });

            salesLineApi.MapGet("/", async (ISalesLineRepository repo) =>
            {
                Console.WriteLine($"Get all saleslines invoked");

                var saleslines = await repo.GetAllSalesLinesAsync();
                return TypedResults.Ok(saleslines);
            });




            salesLineApi.MapPost("/", async (SalesLine salesline, ISalesLineRepository repo) => {

                Console.WriteLine($"Post SalesLine invoked");

                SalesLine? newSalesLine = await repo.Add(salesline);
                return TypedResults.Created($"/api/SalesLine/{newSalesLine?.Id}", newSalesLine);
            });

        }
    }
}
