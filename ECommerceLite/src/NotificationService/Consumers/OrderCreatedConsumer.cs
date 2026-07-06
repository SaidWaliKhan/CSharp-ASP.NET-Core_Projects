using ECommerceLite.Contracts.Events;
using MassTransit;

namespace NotificationService.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvents>
{
    private readonly ILogger<OrderCreatedEvents> _logger;

    public OrderCreatedConsumer(ILogger<OrderCreatedEvents> logger) => _logger = logger;

    public Task Consume(ConsumeContext<OrderCreatedEvents> context)
    {
        var order = context.Message;

        _logger.LogInformation(
            "📩 Notification: Order {OrderId} created for product {ProductId}, qty {Quantity}, total {Total:C}",

             order.OrderId, order.ProductId, order.Quantity, order.TotalPrice
        );

        return Task.CompletedTask;
    }
}

