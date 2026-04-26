using Microsoft.Extensions.Caching.Memory;
using PizzaModels.Models;
using PizzaWebAPI.DTO;
using PizzaWebAPI.Service;

namespace PizzaWebAPI.Endpoints
{
    public static class ProductEndpoint
    {
        private const string AllProductsCacheKey = "all-products";

        public static IEndpointRouteBuilder MapProductEndpoint(this IEndpointRouteBuilder app)
        {
            var productApi = app.MapGroup("/api/products").WithTags("products");

            productApi.MapGet("/{id}", async (int id, IWebServiceProduct ws) =>
            {
                Console.WriteLine("Get product by id invoked");

                var product = await ws.Get(id);
                return TypedResults.Ok(product);
            });

            productApi.MapGet("/", async (IWebServiceProduct ws, IMemoryCache cache) =>
            {
                Console.WriteLine("Get all products invoked");

                if (cache.TryGetValue(AllProductsCacheKey, out List<ProductDTO>? cachedProducts))
                {
                    Console.WriteLine("Products returned from cache");
                    return TypedResults.Ok(cachedProducts);
                }

                Console.WriteLine("Products loaded from backend/database");

                var products = await ws.GetAllProductsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

                cache.Set(AllProductsCacheKey, products, cacheOptions);

                return TypedResults.Ok(products);
            });

            productApi.MapPost("/", static async (ProductDTO productDTO, IWebServiceProduct ws, IMemoryCache cache) =>
            {
                Console.WriteLine("Post product invoked");

                ProductDTO? newProduct = await ws.Add(productDTO);

                // Product list has changed, so the cached menu is no longer valid.
                cache.Remove(AllProductsCacheKey);

                return TypedResults.Created($"/api/products/{newProduct?.Id}", newProduct);
            });

            return app;
        }
    }
}