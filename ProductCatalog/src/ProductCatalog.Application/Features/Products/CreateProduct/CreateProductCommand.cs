using MediatR;
using ProductCatalog.Application.Common.Results;

namespace ProductCatalog.Application.Features.Products.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    decimal Price,
    int Stock,
    string Category) : IRequest<Result<ProductResponse>>;