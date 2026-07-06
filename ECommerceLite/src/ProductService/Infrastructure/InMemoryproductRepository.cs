using ProductService.Domain;

namespace ProductService.Infrastructure;

public class InMemoryProductRepository : IProductRepository
{
    // it is just for testing
    // later we replace it with real DataBase
    private readonly List<Product> _products = new()
    {
    new Product { Id = Guid.NewGuid(), Name = "Mouse", Price = 15.99m, StockQuantity = 50 },
    new Product { Id = Guid.NewGuid(), Name = "Keyboard", Price = 49.99m, StockQuantity = 30 }
    };


    // Get by id
    public Product? GetById(Guid id) => _products.FirstOrDefault(p => p.Id == id);

    public IEnumerable<Product> GetAll() => _products;

    public void Add(Product product) => _products.Add(product);
   
}