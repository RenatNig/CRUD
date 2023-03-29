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
            return _context.Orders;
        }
    }
    
    public IEnumerable<OrderItem> OrderItems
    {
        get
        {
            return _context.OrderItems;
        }
    }
    
    public IEnumerable<Provider> Providers
    {
        get
        {
            return _context.Providers;
        }
    }

}