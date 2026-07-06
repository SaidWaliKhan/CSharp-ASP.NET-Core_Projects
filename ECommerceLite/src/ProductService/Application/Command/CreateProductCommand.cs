using MediatR;

namespace ProductService.Application.Command;

public record CreateProductCommand(
    string Name,
    decimal Price,
    int StockQuantity) : IRequest<Guid>;
