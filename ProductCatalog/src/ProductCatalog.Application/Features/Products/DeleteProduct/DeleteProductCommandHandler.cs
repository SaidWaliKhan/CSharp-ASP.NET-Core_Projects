using MediatR;
using ProductCatalog.Application.Common.Interfaces;
using ProductCatalog.Application.Common.Results;

namespace ProductCatalog.Application.Features.Products.DeleteProduct;

public sealed class DeleteProductCommandHandler(
    IProductRepository repository,
    ICacheService cache) : IRequestHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
            return Result.Failure(ProductErrors.NotFound(request.Id));

        await repository.DeleteAsync(product, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync($"product:{request.Id}", cancellationToken);

        return Result.Success();
    }
}