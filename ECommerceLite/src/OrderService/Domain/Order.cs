namespace OrderService.Domain;

public class Order
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal TotalPrice { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    

}