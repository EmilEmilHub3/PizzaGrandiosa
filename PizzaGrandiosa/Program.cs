using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using PizzaGrandiosa.Endpoints;
using PizzaGrandiosa.Persistence;
using PizzaGrandiosa.Repositories;
using PizzaGrandiosa.Services;


namespace PizzaGrandiosa;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Services
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<PizzaDbContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection is missing in appsettings.json");

            options.UseNpgsql(connectionString);
        });

        builder.Services.Configure<RabbitMqOptions>(
            builder.Configuration.GetSection(RabbitMqOptions.SectionName));

        builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
        builder.Services.AddScoped<ISalesLineRepository, SalesLineRepository>();

        var app = builder.Build();

        // Middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            await using var scope = app.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PizzaDbContext>();

            await dbContext.Database.EnsureCreatedAsync();
            Console.WriteLine("Database ensured/created");
        }

        app.UseAuthorization();

        // Endpoints
        app.MapGet("/", () => "Hello World!")
           .Produces(200, typeof(string));

        app.MapUserEndpoints();
        app.MapCustomerEndpoints();
        app.MapProductEndpoints();
        app.MapSalesOrderEndpoints();
        app.MapSalesLilneEndpoints();

        app.Run();
    }
}