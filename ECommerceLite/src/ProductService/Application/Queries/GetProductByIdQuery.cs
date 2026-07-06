using ProductService.Domain;
using MediatR;

namespace ProductService.Application.Queries;

public record GetProductByIdQuery
(
    Guid Id

) : IRequest<Product?>;