using ProductService.Domain;
using ProductService.Infrastructure;
using MediatR;

namespace ProductService.Application.Command;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _repo;

    public CreateProductCommandHandler(IProductRepository repository)
    {
        _repo = repository;
    }

    public Task<Guid> Handle (CreateProductCommand request, CancellationToken token)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            StockQuantity = request.StockQuantity

        };

        _repo.Add(product);

        return Task.FromResult(product.Id);
    }
}