using Microsoft.EntityFrameworkCore;
using CRUD.Data.Abstract;
using CRUD.Models;

namespace CRUD.Data;

public class EFRepository : IRepository
{
    public EFDbContext _context;

    public IEnumerable<Order> Orders
    {
        get
        {
            return _context.Order;
        }
    }
    
    public IEnumerable<OrderItem> OrderItems
    {
        get
        {
            return _context.OrderItem;
        }
    }
    
    public IEnumerable<Provider> Providers
    {
        get
        {
            return _context.Provider;
        }
    }

    public void Add(Order order)
    {
        _context.Add(order);
    }
    
    public void Add(OrderItem orderItem)
    {
        _context.Add(orderItem);
    }
    
    public void Update(Order order)
    {
        _context.Update(order);
    }
    
    public void Update(OrderItem orderItem)
    {
        _context.Update(orderItem);
    }
    
    public void Remove(Order order)
    {
        _context.Order.Remove(order);
    }
    
    public void Remove(OrderItem orderItem)
    {
        _context.OrderItem.Remove(orderItem);
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }

    public Order FindOrder(int id)
    {
        return _context.Order.Find(id);
    }
    
    public OrderItem FindOrderItem(int id)
    {
        return _context.OrderItem.Find(id);
    }

}