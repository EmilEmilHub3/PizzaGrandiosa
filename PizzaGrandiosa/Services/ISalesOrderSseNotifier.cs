using PizzaGrandiosa.Contracts;

namespace PizzaGrandiosa.Services
{
    public interface ISalesOrderSseNotifier
    {
        IAsyncEnumerable<SalesOrderStatusMessage> StreamOrderStatusAsync(
            int salesOrderId,
            CancellationToken cancellationToken);

        ValueTask PublishAsync(
            SalesOrderStatusMessage message,
            CancellationToken cancellationToken = default);
    }
}