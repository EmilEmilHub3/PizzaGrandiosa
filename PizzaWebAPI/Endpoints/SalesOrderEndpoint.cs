using PizzaWebAPI.DTO;
using PizzaWebAPI.Service;

namespace PizzaWebAPI.Endpoints
{
    public static class SalesOrderEndpoint
    {
        public static IEndpointRouteBuilder MapSalesOrderEndpoint(this IEndpointRouteBuilder app)
        {
            var salesOrderApi = app.MapGroup("/api/salesorder").WithTags("salesorder");

            salesOrderApi.MapGet("/{id}", async Task<IResult> (
                int id,
                IWebServiceSalesOrder ws) =>
            {
                Console.WriteLine("Get salesorder by id invoked");

                var salesorder = await ws.Get(id);

                if (salesorder is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(salesorder);
            });

            salesOrderApi.MapGet("/", async (
                IWebServiceSalesOrder ws) =>
            {
                Console.WriteLine("Get all salesorders invoked");

                var salesorders = await ws.GetAllSalesOrdersAsync();

                return Results.Ok(salesorders);
            });

            salesOrderApi.MapPost("/", async (
                SalesOrderDTO salesOrder,
                IWebServiceSalesOrder ws) =>
            {
                Console.WriteLine("Post salesorder invoked");

                SalesOrderDTO? newSalesOrder = await ws.Add(salesOrder);

                return Results.Created(
                    $"/api/salesorder/{newSalesOrder?.Id}",
                    newSalesOrder);
            });

            salesOrderApi.MapPut("/{id}/accept", async Task<IResult> (
                int id,
                IWebServiceSalesOrder ws) =>
            {
                Console.WriteLine($"Accept salesorder invoked for order {id}");

                var status = await ws.Accept(id);

                if (status is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(status);
            });

            salesOrderApi.MapGet("/{id}/status-stream", async (
                int id,
                HttpContext context,
                IHttpClientFactory httpClientFactory) =>
            {
                Console.WriteLine($"SSE status stream proxy invoked for order {id}");

                context.Response.Headers.Append("Content-Type", "text/event-stream");
                context.Response.Headers.Append("Cache-Control", "no-cache");
                context.Response.Headers.Append("Connection", "keep-alive");

                var client = httpClientFactory.CreateClient("Default");
                client.Timeout = Timeout.InfiniteTimeSpan;

                using var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"/api/salesorder/{id}/status-stream");

                using var response = await client.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    context.RequestAborted);

                if (!response.IsSuccessStatusCode)
                {
                    context.Response.StatusCode = (int)response.StatusCode;
                    return;
                }

                await using var stream =
                    await response.Content.ReadAsStreamAsync(context.RequestAborted);

                await stream.CopyToAsync(context.Response.Body, context.RequestAborted);
            });

            return app;
        }
    }
}