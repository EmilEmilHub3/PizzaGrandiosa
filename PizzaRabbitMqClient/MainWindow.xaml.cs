using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace PizzaRabbitMqClient;

public partial class MainWindow : Window
{
    private const string QueueName = "salesorder-created";

    private IConnection? _connection;
    private IChannel? _channel;

    public ObservableCollection<SalesOrderCreatedMessage> Orders { get; } = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        Loaded += MainWindow_Loaded;
        Closing += MainWindow_Closing;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(
                queue: QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += Consumer_ReceivedAsync;

            await _channel.BasicConsumeAsync(
                queue: QueueName,
                autoAck: true,
                consumer: consumer);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Kunne ikke starte RabbitMQ-klienten.\n\n{ex.Message}",
                "RabbitMQ fejl",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs ea)
    {
        try
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            var order = JsonSerializer.Deserialize<SalesOrderCreatedMessage>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (order != null)
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    Orders.Insert(0, order);
                });
            }
        }
        catch
        {
            // Ignorer fejlende enkeltbeskeder i prototypen
        }
    }

    private async void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }

            if (_connection != null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }
        }
        catch
        {
            // Ignorer lukke-fejl i prototypen
        }
    }
}