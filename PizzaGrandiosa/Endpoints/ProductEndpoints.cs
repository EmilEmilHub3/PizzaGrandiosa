//using PizzaGrandiosa.Models;
using PizzaGrandiosa.Repositories;
using PizzaModels.Models;

namespace PizzaGrandiosa.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
        {
            var productApi = routes.MapGroup("/api/product").WithTags("product");

            productApi.MapGet("/{id}", async (IProductRepository repo, int id) =>
            {
                Console.WriteLine($"Get product by id invoked");

                var product = await repo.Get(id);
                return TypedResults.Ok(product);
            });

            productApi.MapGet("/", async (IProductRepository repo) =>
            {
                Console.WriteLine($"Get all products invoked");

                var product = await repo.GetAllProductsAsync();
                return TypedResults.Ok(product);
            });


            productApi.MapPost("/", async (Product product, IProductRepository repo) => {

                Console.WriteLine($"Post product invoked");

                Product? newProduct = await repo.Add(product);
                return TypedResults.Created($"/api/product/{newProduct?.Id}", newProduct);
            });

        }
    }
}
