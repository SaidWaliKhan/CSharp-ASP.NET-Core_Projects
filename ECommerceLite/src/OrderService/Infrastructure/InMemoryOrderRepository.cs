using OrderService.Domain;

namespace OrderService.Infrastructure;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();

    public void Add(Order order) => _orders.Add(order);
    public IEnumerable<Order> GetAll() => _orders;
}