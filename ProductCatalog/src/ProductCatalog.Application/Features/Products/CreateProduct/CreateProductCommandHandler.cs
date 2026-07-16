using MediatR;
using ProductCatalog.Application.Common.Interfaces;
using ProductCatalog.Application.Common.Results;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Features.Products.CreateProduct;

public sealed class CreateProductCommandHandler(
    IProductRepository repository) : IRequestHandler<CreateProductCommand, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsByNameAsync(request.Name, cancellationToken);

        if (exists)
            return Result.Failure<ProductResponse>(ProductErrors.DuplicateName(request.Name));

        var product = new Product(request.Name, request.Price, request.Stock, request.Category);

        await repository.AddAsync(product, cancellationToken);
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