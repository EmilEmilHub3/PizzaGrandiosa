namespace PizzaRabbitMqClient;

public class SalesOrderCreatedMessage
{
    public int SalesOrderId { get; set; }
    public int CustomerId { get; set; }
    public string? OrderType { get; set; }
    public bool IsAccepted { get; set; }
    public bool IsPosted { get; set; }
    public DateTime Date { get; set; }
    public int SalesLineCount { get; set; }
}
