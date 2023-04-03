using CRUD.Models;

namespace CRUD.Data.Abstract;

public interface IRepository
{
    public IEnumerable<Order> Orders { get; }
    public IEnumerable<OrderItem> OrderItems { get; }
    public IEnumerable<Provider> Providers { get; }

    public void Add(Order order);
    public void Add(OrderItem orderItem);
    
    public void Update(Order order);
    public void Update(OrderItem orderItem);
    
    public void Remove(Order order);
    public void Remove(OrderItem orderItem);
    
    public Order FindOrder(int id);
    public OrderItem FindOrderItem(int id);
    
    public Task Save();
}