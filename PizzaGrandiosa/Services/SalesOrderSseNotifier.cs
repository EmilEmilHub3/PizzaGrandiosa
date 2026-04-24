
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using PizzaGrandiosa.Contracts;

namespace PizzaGrandiosa.Services
{
    public class SalesOrderSseNotifier : ISalesOrderSseNotifier
    {
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<Guid, Channel<SalesOrderStatusMessage>>> _subscribers = new();

        public async IAsyncEnumerable<SalesOrderStatusMessage> StreamOrderStatusAsync(
            int salesOrderId,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var channel = Channel.CreateUnbounded<SalesOrderStatusMessage>();
            var subscriberId = Guid.NewGuid();

            var orderSubscribers = _subscribers.GetOrAdd(
                salesOrderId,
                _ => new ConcurrentDictionary<Guid, Channel<SalesOrderStatusMessage>>());

            orderSubscribers[subscriberId] = channel;

            try
            {
                while (await channel.Reader.WaitToReadAsync(cancellationToken))
                {
                    while (channel.Reader.TryRead(out var message))
                    {
                        yield return message;
                    }
                }
            }
            finally
            {
                if (_subscribers.TryGetValue(salesOrderId, out var subscribers))
                {
                    subscribers.TryRemove(subscriberId, out _);

                    if (subscribers.IsEmpty)
                    {
                        _subscribers.TryRemove(salesOrderId, out _);
                    }
                }
            }
        }

        public ValueTask PublishAsync(
            SalesOrderStatusMessage message,
            CancellationToken cancellationToken = default)
        {
            if (_subscribers.TryGetValue(message.SalesOrderId, out var subscribers))
            {
                foreach (var subscriber in subscribers.Values)
                {
                    subscriber.Writer.TryWrite(message);
                }
            }

            return ValueTask.CompletedTask;
        }
    }
}