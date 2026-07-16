using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Infrastructure.Persistence;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 65000,
                Stock = 10,
                Category = "Electronics"
            },
            new Product
            {
                Id = 2,
                Name = "Mouse",
                Price = 1200,
                Stock = 25,
                Category = "Electronics"
            },
            new Product
            {
                Id = 3,
                Name = "Keyboard",
                Price = 2500,
                Stock = 15,
                Category = "Electronics"
            }
        );
    }

}