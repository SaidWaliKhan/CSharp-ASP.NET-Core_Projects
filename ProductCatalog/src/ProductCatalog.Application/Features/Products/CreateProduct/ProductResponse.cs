namespace ProductCatalog.Application.Features.Products.CreateProduct;

public sealed record ProductResponse(
    int Id,
    string Name,
    decimal Price,
    int Stock,
    string Category);