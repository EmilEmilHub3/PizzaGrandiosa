using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaModels.Models;
//using PizzaGrandiosa.Models;

namespace PizzaGrandiosa.Persistence.Configurations
{
    /*
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Define table name
            builder.ToTable("Users");

            // Set primary key
            builder.HasKey(m => m.Id);

            // Configure properties of the Model class
            builder.Property(m => m.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(m => m.UserName)
                   .IsRequired()
                   .HasMaxLength(100);
    

            // Configure Created and LastModified properties to be handled as immutable and modifiable timestamps
            builder.Property(m => m.Created)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(m => m.LastModified)
                   .IsRequired()
                   .ValueGeneratedOnUpdate();

            // Optional: Add indexes for better query performance
            builder.HasIndex(m => m.Name);
        }
    }
    */
}
