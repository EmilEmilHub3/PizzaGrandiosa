using PizzaWebAPI.Endpoints;
using PizzaWebAPI.Service;

namespace PizzaWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "MyAllowedOrigins",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:80", "http://localhost")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient(
                "Default",
                client =>
                {
                    client.BaseAddress = new Uri(builder.Configuration["ApiSetting:DBServerURL"]);
                });

            builder.Services.AddScoped<IWebServiceUser, WebServiceUser>();
            builder.Services.AddScoped<IWebServiceProduct, WebServiceProduct>();
            builder.Services.AddScoped<IWebServiceCustomer, WebServiceCustomer>();
            builder.Services.AddScoped<IWebServiceSalesLine, WebServiceSalesLine>();
            builder.Services.AddScoped<IWebServiceSalesOrder, WebServiceSalesOrder>();

            var app = builder.Build();

            app.UseCors("MyAllowedOrigins");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapGet("/", () => "Hello world from the core web API");

            app.MapUserEndpoint();
            app.MapCustomerEndpoint();
            app.MapProductEndpoint();
            app.MapSalesLineEndpoint();
            app.MapSalesOrderEndpoint();

            app.Run();
        }
    }
}