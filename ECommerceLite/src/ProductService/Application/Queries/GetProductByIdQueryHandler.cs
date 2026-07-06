using ProductService.Domain;
using ProductService.Infrastructure;
using MediatR;

namespace ProductService.Application.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product?>
{
    private readonly IProductRepository _repository;

    public GetProductByIdQueryHandler(IProductRepository repository) => _repository = repository;

    public Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_repository.GetById(request.Id));
    }
}