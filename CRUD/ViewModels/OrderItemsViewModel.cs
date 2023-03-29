using CRUD.Models;

namespace CRUD.ViewModels
{
    public class OrderItemsViewModel
    {
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}