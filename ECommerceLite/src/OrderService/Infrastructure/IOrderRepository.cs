using OrderService.Domain;

namespace OrderService.Infrastructure;

public interface IOrderRepository
{
    void Add(Order order);
    IEnumerable<Order> GetAll();
}