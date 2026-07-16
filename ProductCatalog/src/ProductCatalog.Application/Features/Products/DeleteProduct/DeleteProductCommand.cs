using MediatR;
using ProductCatalog.Application.Common.Results;

namespace ProductCatalog.Application.Features.Products.DeleteProduct;

public sealed record DeleteProductCommand(int Id) : IRequest<Result>;