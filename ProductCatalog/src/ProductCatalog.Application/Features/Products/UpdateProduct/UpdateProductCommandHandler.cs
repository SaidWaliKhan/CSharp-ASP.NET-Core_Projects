using MediatR;
using ProductCatalog.Application.Common.Interfaces;
using ProductCatalog.Application.Common.Results;
using ProductCatalog.Application.Features.Products.CreateProduct;

namespace ProductCatalog.Application.Features.Products.UpdateProduct;

public sealed class UpdateProductCommandHandler(
    IProductRepository repository) : IRequestHandler<UpdateProductCommand, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
            return Result.Failure<ProductResponse>(ProductErrors.NotFound(request.Id));

        product.Rename(request.Name);
        product.UpdatePrice(request.Price);
        product.UpdateStock(request.Stock);
        product.ChangeCategory(request.Category);

        await repository.UpdateAsync(product, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        var response = new ProductResponse(
            product.Id,
            product.Name,
            product.Price,
            product.Stock,
            product.Category);

        return Result.Success(response);
    }
}