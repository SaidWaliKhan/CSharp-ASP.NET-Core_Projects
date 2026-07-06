using ProductService.Domain;

namespace ProductService.Infrastructure;

public interface IProductRepository
{
    Product? GetById(Guid id);
    IEnumerable<Product> GetAll();
    void Add(Product product);
}