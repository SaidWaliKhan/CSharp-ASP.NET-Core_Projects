namespace ECommerceLite.Contracts.Events;

public record OrderCreatedEvents(
    Guid OrderId,
    Guid ProductId,
    int Quantity,
    decimal TotalPrice,
    DateTime CreatedAtUtc);