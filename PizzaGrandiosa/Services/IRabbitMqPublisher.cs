using PizzaGrandiosa.Contracts;

namespace PizzaGrandiosa.Services
{
    public interface IRabbitMqPublisher
    {
        Task PublishSalesOrderCreatedAsync(SalesOrderCreatedMessage message);
    }
}
