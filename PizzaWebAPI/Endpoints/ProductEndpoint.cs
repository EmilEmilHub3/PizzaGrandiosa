using PizzaModels.Models;
using PizzaWebAPI.DTO;
using PizzaWebAPI.Service;

namespace PizzaWebAPI.Endpoints
{
    public static class ProductEndpoint
    {
        public static IEndpointRouteBuilder MapProductEndpoint(this IEndpointRouteBuilder app) //WebApplication
        {
            var productApi = app.MapGroup("/api/products").WithTags("products");

            productApi.MapGet("/{id}", async (int id, IWebServiceProduct ws) =>
            {
                Console.WriteLine($"Get product by id invoked");

                var product = await ws.Get(id);
                return TypedResults.Ok(product);
            });

            productApi.MapGet("/", async (IWebServiceProduct ws) =>
            {
                Console.WriteLine($"Get all products invoked");

                var products = await ws.GetAllProductsAsync();
                return TypedResults.Ok(products);
            });


            productApi.MapPost("/", static async (ProductDTO productDTO, IWebServiceProduct ws) => {

                Console.WriteLine($"Post product invoked");

                ProductDTO? newProduct = await ws.Add(productDTO);
                return TypedResults.Created($"/api/products/{newProduct?.Id}", newProduct);
            });

            return app;
        }
    }
}
