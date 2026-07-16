using ProductService.Domain;
using MediatR;

namespace ProductService.Application.Queries;

public record GetAllProductsQuery(

) : IRequest<IEnumerable<Product>>;