using ProductCatalog.Application.Common.Errors;

namespace ProductCatalog.Application.Common.Results;

public static class ProductErrors
{
    public static Error NotFound(int id) =>
        new("Product.NotFound", $"Product with ID {id} was not found.");

    public static Error DuplicateName(string name) =>
        new("Product.DuplicateName", $"A product with name '{name}' already exists.");

    public static Error InvalidPrice() =>
        new("Product.InvalidPrice", "Price must be greater than zero.");

    public static Error InvalidStock() =>
        new("Product.InvalidStock", "Stock cannot be negative.");
}