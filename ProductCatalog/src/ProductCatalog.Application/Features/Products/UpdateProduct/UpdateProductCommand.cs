using MediatR;
using ProductCatalog.Application.Common.Results;
using ProductCatalog.Application.Features.Products.CreateProduct;

namespace ProductCatalog.Application.Features.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    int Id,
    string Name,
    decimal Price,
    int Stock,
    string Category) : IRequest<Result<ProductResponse>>;