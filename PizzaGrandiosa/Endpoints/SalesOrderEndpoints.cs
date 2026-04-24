using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PizzaGrandiosa.Contracts;
using PizzaGrandiosa.Repositories;
using PizzaGrandiosa.Services;
using PizzaModels.Models;

namespace PizzaGrandiosa.Endpoints
{
    public static class SalesOrderEndpoints
    {
        private static readonly JsonSerializerOptions SseJsonOptions =
            new(JsonSerializerDefaults.Web);

        public static void MapSalesOrderEndpoints(this IEndpointRouteBuilder routes)
        {
            var salesOrderApi =
                routes.MapGroup("/api/salesorder")
                .WithTags("salesorder");

            salesOrderApi.MapGet("/{id}", async Task<IResult> (
                ISalesOrderRepository repo,
                int id) =>
            {
                Console.WriteLine("Get salesOrder by id invoked");

                var salesOrder = await repo.Get(id);

                if (salesOrder is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(salesOrder);
            });

            salesOrderApi.MapGet("/", async (
                ISalesOrderRepository repo) =>
            {
                Console.WriteLine("Get all salesOrder invoked");

                var salesOrders =
                    await repo.GetAllSalesOrdersAsync();

                return Results.Ok(salesOrders);
            });

            salesOrderApi.MapPost("/", async (
                SalesOrder salesOrder,
                ISalesOrderRepository repo,
                IRabbitMqPublisher rabbitMqPublisher) =>
            {
                Console.WriteLine("Post SalesOrder invoked");

                SalesOrder? newSalesOrder =
                    await repo.Add(salesOrder);

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

                        SalesLines =
                            newSalesOrder.SalesLines?
                            .Select(sl => new SalesLineMessage
                            {
                                Id = sl.Id,
                                SalesOrderId = sl.SalesOrderId,
                                Quantity = sl.Quantity,
                                Price = sl.Price,
                                ProductId = sl.ProductId,

                                Product = sl.Product == null
                                    ? null
                                    : new ProductMessage
                                    {
                                        Id = sl.Product.Id,
                                        Type = sl.Product.Type,
                                        Description = sl.Product.Description,
                                        Price = sl.Product.Price
                                    }

                            }).ToList()
                            ?? new List<SalesLineMessage>()
                    };

                    await rabbitMqPublisher
                        .PublishSalesOrderCreatedAsync(message);
                }

                return Results.Created(
                    $"/api/salesorder/{newSalesOrder?.Id}",
                    newSalesOrder);
            });

            salesOrderApi.MapPut("/{id}/accept", async Task<IResult> (
                int id,
                ISalesOrderRepository repo,
                ISalesOrderSseNotifier sseNotifier) =>
            {
                Console.WriteLine($"Accept SalesOrder invoked for order {id}");

                var acceptedOrder =
                    await repo.MarkAcceptedAsync(id);

                if (acceptedOrder is null)
                {
                    return Results.NotFound();
                }

                var statusMessage =
                    ToStatusMessage(acceptedOrder);

                await sseNotifier.PublishAsync(statusMessage);

                // FIXED: Return DTO instead of EF entity
                return Results.Ok(statusMessage);
            });

            salesOrderApi.MapGet("/{id}/status-stream", async (
                int id,
                HttpContext context,
                ISalesOrderRepository repo,
                ISalesOrderSseNotifier sseNotifier) =>
            {
                var existingOrder =
                    await repo.Get(id);

                if (existingOrder is null)
                {
                    context.Response.StatusCode =
                        StatusCodes.Status404NotFound;
                    return;
                }

                context.Response.Headers.Append(
                    "Content-Type",
                    "text/event-stream");

                context.Response.Headers.Append(
                    "Cache-Control",
                    "no-cache");

                context.Response.Headers.Append(
                    "Connection",
                    "keep-alive");

                await WriteSseEventAsync(
                    context.Response,
                    "order-status",
                    ToStatusMessage(existingOrder),
                    context.RequestAborted);

                await foreach (var statusUpdate in
                    sseNotifier.StreamOrderStatusAsync(
                        id,
                        context.RequestAborted))
                {
                    await WriteSseEventAsync(
                        context.Response,
                        "order-status",
                        statusUpdate,
                        context.RequestAborted);
                }
            });
        }

        private static SalesOrderStatusMessage ToStatusMessage(
            SalesOrder order)
        {
            return new SalesOrderStatusMessage
            {
                SalesOrderId = order.Id,
                CustomerId = order.CustomerId,
                IsAccepted = order.IsAccepted,
                IsPosted = order.IsPosted,
                Status =
                    order.IsAccepted
                        ? "Accepted"
                        : "Pending",

                UpdatedAtUtc = DateTime.UtcNow
            };
        }

        private static async Task WriteSseEventAsync<T>(
            HttpResponse response,
            string eventName,
            T payload,
            CancellationToken cancellationToken)
        {
            var json =
                JsonSerializer.Serialize(
                    payload,
                    SseJsonOptions);

            await response.WriteAsync(
                $"event: {eventName}\n",
                cancellationToken);

            await response.WriteAsync(
                $"data: {json}\n\n",
                cancellationToken);

            await response.Body.FlushAsync(
                cancellationToken);
        }
    }
}