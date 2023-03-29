using CRUD.Models;

namespace CRUD.Data.Abstract;

public interface IRepository
{
    public IEnumerable<Order> Orders { get; }
    public IEnumerable<OrderItem> OrderItems { get; }
    public IEnumerable<Provider> Providers { get; }
}