using MediatR;
using ProductCatalog.Application.Common.Results;
using ProductCatalog.Application.Features.Products.CreateProduct;

namespace ProductCatalog.Application.Features.Products.GetProductById;

public sealed record GetProductByIdQuery(int Id) : IRequest<Result<ProductResponse>>;