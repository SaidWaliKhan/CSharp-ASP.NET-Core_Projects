using ECommerceLite.Contracts.Events;
using MassTransit;
using MediatR;
using OrderService.Domain;
using OrderService.Infrastructure;

namespace OrderService.Command;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _repository;
    private readonly ProductServiceClient _productServiceClient;
    private readonly IPublishEndpoint _publishEndpoint; // MassTransit's "send to RabbitMQ" API

    public CreateOrderCommandHandler(
        IOrderRepository repository,
        ProductServiceClient productServiceClient,
        IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _productServiceClient = productServiceClient;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var productExists = await _productServiceClient.ProductExistsAsync(
            request.ProductId, request.BearerToken);

        if (!productExists)
            throw new InvalidOperationException("Product does not exist.");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            TotalPrice = request.TotalPrice,
            CreatedAtUtc = DateTime.UtcNow
        };

        _repository.Add(order);

        // This is the async, event-driven part — OrderService doesn't call
        // NotificationService directly. It just publishes a fact ("this happened")
        // and RabbitMQ delivers it to whoever is listening.
        await _publishEndpoint.Publish(new
        OrderCreatedEvents(
            order.Id,
            order.ProductId,
            order.Quantity,
            order.TotalPrice,
            order.CreatedAtUtc),
            cancellationToken);

        return order.Id;
    }
}    