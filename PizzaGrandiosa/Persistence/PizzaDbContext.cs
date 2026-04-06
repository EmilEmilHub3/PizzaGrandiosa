using Microsoft.EntityFrameworkCore;
using PizzaModels.Models;
//using PizzaGrandiosa.Models;

namespace PizzaGrandiosa.Persistence
{
    public class PizzaDbContext(DbContextOptions<PizzaDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
        public DbSet<SalesLine> SalesLines => Set<SalesLine>();

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PizzaDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    var sampleUser = await context.Set<User>().FirstOrDefaultAsync(u => u.Name == "LeifOve", cancellationToken);
                    if (sampleUser == null)
                    {
                        sampleUser = User.Create("LeifOve", "LeifOve user");
                        await context.Set<User>().AddAsync(sampleUser, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);
                    }
                })
                .UseSeeding((context, _) =>
                {
                    var sampleUser = context.Set<User>().FirstOrDefault(u => u.Name == "LeifOve");
                    if (sampleUser == null)
                    {
                        sampleUser = User.Create("LeifOve", "LeifOve user");
                        context.Set<User>().Add(sampleUser);
                        context.SaveChanges();
                    }
                });
        }
        */
    }
}
    