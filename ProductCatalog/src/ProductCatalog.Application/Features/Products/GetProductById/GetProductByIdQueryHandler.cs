using MediatR;
using ProductCatalog.Application.Common.Interfaces;
using ProductCatalog.Application.Common.Results;
using ProductCatalog.Application.Features.Products.CreateProduct;
using System.Text.Json;

namespace ProductCatalog.Application.Features.Products.GetProductById;

public sealed class GetProductByIdQueryHandler(
    IProductRepository repository,
    ICacheService cache) : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"product:{request.Id}";

        var cached = await cache.GetAsync<string>(cacheKey, cancellationToken);

        if (cached is not null)
        {
            var cachedProduct = JsonSerializer.Deserialize<ProductResponse>(cached);
            return Result.Success(cachedProduct!);
        }

        var product = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
            return Result.Failure<ProductResponse>(ProductErrors.NotFound(request.Id));

        var response = new ProductResponse(
            product.Id,
            product.Name,
            product.Price,
            product.Stock,
            product.Category);

        await cache.SetAsync(cacheKey, JsonSerializer.Serialize(response), TimeSpan.FromMinutes(10), cancellationToken);

        return Result.Success(response);
    }
}