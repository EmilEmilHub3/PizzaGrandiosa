using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Windows;

namespace PizzaRabbitMqClient;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<SalesOrderCreatedMessage> _orders = new();

    public MainWindow()
    {
        InitializeComponent();
        OrdersGrid.ItemsSource = _orders;
        _ = StartListeningAsync();
    }

    private async Task StartListeningAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "salesorder-created",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonSerializer.Deserialize<SalesOrderCreatedMessage>(json);

            if (message is not null)
            {
                await Dispatcher.InvokeAsync(() => _orders.Insert(0, message));
            }
        };

        await channel.BasicConsumeAsync(
            queue: "salesorder-created",
            autoAck: true,
            consumer: consumer);
    }
}
