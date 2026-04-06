using PizzaGrandiosa.Contracts;
using PizzaGrandiosa.Repositories;
using PizzaGrandiosa.Services;
using PizzaModels.Models;

namespace PizzaGrandiosa.Endpoints
{
    public static class SalesOrderEndpoints
    {
        public static void MapSalesOrderEndpoints(this IEndpointRouteBuilder routes)
        {
            var salesOrderApi = routes.MapGroup("/api/salesorder").WithTags("salesorder");

            salesOrderApi.MapGet("/{id}", async (ISalesOrderRepository repo, int id) =>
            {
                Console.WriteLine("Get salesOrder by id invoked");

                var salesOrder = await repo.Get(id);
                return TypedResults.Ok(salesOrder);
            });

            salesOrderApi.MapGet("/", async (ISalesOrderRepository repo) =>
            {
                Console.WriteLine("Get all salesOrder invoked");

                var salesOrders = await repo.GetAllSalesOrdersAsync();
                return TypedResults.Ok(salesOrders);
            });

            salesOrderApi.MapPost("/", async (
                SalesOrder salesOrder,
                ISalesOrderRepository repo,
                IRabbitMqPublisher rabbitMqPublisher) =>
            {
                Console.WriteLine("Post SalesOrder invoked");

                SalesOrder? newSalesOrder = await repo.Add(salesOrder);

                if (newSalesOrder is not null)
                {
                    var message = new SalesOrderCreatedMessage
                    {
                        SalesOrderId = newSalesOrder.Id,
                        CustomerId = newSalesOrder.CustomerId,
                        OrderType = newSalesOrder.OrderType,
                        IsAccepted = newSalesOrder.IsAccepted,
                        IsPosted = newSalesOrder.IsPosted,
                        Date = newSalesOrder.Date,
                        SalesLines = newSalesOrder.SalesLines?.Select(sl => new SalesLineMessage
                        {
                            Id = sl.Id,
                            SalesOrderId = sl.SalesOrderId,
                            Quantity = sl.Quantity,
                            Price = sl.Price,
                            ProductId = sl.ProductId,
                            Product = sl.Product == null ? null : new ProductMessage
                            {
                                Id = sl.Product.Id,
                                Type = sl.Product.Type,
                                Description = sl.Product.Description,
                                Price = sl.Product.Price
                            }
                        }).ToList() ?? new List<SalesLineMessage>()
                    };

                    await rabbitMqPublisher.PublishSalesOrderCreatedAsync(message);
                }

                return TypedResults.Created($"/api/customer/{newSalesOrder?.Id}", newSalesOrder);
            });
        }
    }
}