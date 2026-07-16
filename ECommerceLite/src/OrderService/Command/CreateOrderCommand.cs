using MediatR;

namespace OrderService.Command;

public record CreateOrderCommand(
    Guid ProductId,
    int Quantity,
    decimal TotalPrice,
    string BearerToken
    ): IRequest<Guid>;