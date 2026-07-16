using ProductCatalog.Domain.Exceptions;

namespace ProductCatalog.Domain.Entities;

public class Product
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public decimal Price { get; private set; }

    public int Stock { get; private set; }

    public string Category { get; private set; }

    private Product()
    {
        Name = string.Empty;
        Category = string.Empty;
    }

    public Product(
        string name,
        decimal price,
        int stock,
        string category)
    {
        SetName(name);
        SetPrice(price);
        SetStock(stock);
        SetCategory(category);
    }

    public void Rename(string name)
    {
        SetName(name);
    }

    public void UpdatePrice(decimal price)
    {
        SetPrice(price);
    }

    public void UpdateStock(int stock)
    {
        SetStock(stock);
    }

    public void ChangeCategory(string category)
    {
        SetCategory(category);
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name is required.");

        Name = name.Trim();
    }

    private void SetPrice(decimal price)
    {
        if (price <= 0)
            throw new DomainException("Price must be greater than zero.");

        Price = price;
    }

    private void SetStock(int stock)
    {
        if (stock < 0)
            throw new DomainException("Stock cannot be negative.");

        Stock = stock;
    }

    private void SetCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new DomainException("Category is required.");

        Category = category.Trim();
    }
}