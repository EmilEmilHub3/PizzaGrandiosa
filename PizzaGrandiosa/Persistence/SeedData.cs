using PizzaModels.Models;

namespace PizzaGrandiosa.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(PizzaDbContext context)
    {
        if (!context.Customers.Any())
        {
            var customers = new List<Customer>
            {
                new Customer { Name = "Emil" },
                new Customer { Name = "Test Kunde" }
            };

            context.Customers.AddRange(customers);
        }

        if (!context.Products.Any())
        {
            var products = new List<Product>
            {
                new Product { Type = "Pizza", Description = "Margherita", Price = 90 },
                new Product { Type = "Pizza", Description = "Pepperoni", Price = 100 },
                new Product { Type = "Pizza", Description = "Hawaii", Price = 95 }
            };

            context.Products.AddRange(products);
        }

        await context.SaveChangesAsync();

        Console.WriteLine("Seed data checked/created");
    }
}