using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order? Order { get; set; }
    [DisplayName("Наименование товара")]
    public string Name { get; set; }
    [Column(TypeName = "decimal(18,3)")] 
    [DisplayName("Количество")]
    public decimal Quantity { get; set; }
    [DisplayName("Модель")]
    public string Unit { get; set; }
}