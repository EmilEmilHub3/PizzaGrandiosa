using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PizzaGrandiosa.Contracts;
using RabbitMQ.Client;

namespace PizzaGrandiosa.Services
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly RabbitMqOptions _options;

        public RabbitMqPublisher(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }

        public async Task PublishSalesOrderCreatedAsync(SalesOrderCreatedMessage message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: _options.QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: _options.QueueName,
                body: body);
        }
    }
}
