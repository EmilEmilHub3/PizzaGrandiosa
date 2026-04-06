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

                var salesOrder = await repo.GetAllSalesOrdersAsync();
                return TypedResults.Ok(salesOrder);
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
                    await rabbitMqPublisher.PublishSalesOrderCreatedAsync(new SalesOrderCreatedMessage
                    {
                        SalesOrderId = newSalesOrder.Id,
                        CustomerId = newSalesOrder.CustomerId,
                        OrderType = newSalesOrder.OrderType,
                        IsAccepted = newSalesOrder.IsAccepted,
                        IsPosted = newSalesOrder.IsPosted,
                        Date = newSalesOrder.Date,
                        SalesLineCount = newSalesOrder.SalesLines?.Count ?? 0
                    });
                }

                return TypedResults.Created($"/api/customer/{newSalesOrder?.Id}", newSalesOrder);
            });
        }
    }
}
