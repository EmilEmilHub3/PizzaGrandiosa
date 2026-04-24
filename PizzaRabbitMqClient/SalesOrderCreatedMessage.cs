using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PizzaRabbitMqClient;

public class SalesOrderCreatedMessage : INotifyPropertyChanged
{
    private bool _isAccepted;

    public int SalesOrderId { get; set; }
    public int CustomerId { get; set; }
    public string? OrderType { get; set; }

    public bool IsAccepted
    {
        get => _isAccepted;
        set
        {
            if (_isAccepted == value)
            {
                return;
            }

            _isAccepted = value;
            OnPropertyChanged();
        }
    }

    public bool IsPosted { get; set; }
    public DateTime Date { get; set; }
    public List<SalesLineMessage> SalesLines { get; set; } = new();

    public int LineCount => SalesLines?.Count ?? 0;

    public string ProductsSummary =>
        SalesLines != null && SalesLines.Any()
            ? string.Join(", ", SalesLines.Select(l =>
                $"{(l.Product?.Description ?? $"ProductId: {l.ProductId}")} x{l.Quantity}"))
            : "Ingen linjer";

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class SalesLineMessage
{
    public int Id { get; set; }
    public int SalesOrderId { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
    public int ProductId { get; set; }
    public ProductMessage? Product { get; set; }
}

public class ProductMessage
{
    public int Id { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
}