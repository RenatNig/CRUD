using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Models;

[Index(nameof(Number), nameof(ProviderId), IsUnique = true)]
public class Order
{
    public int Id { get; set; }
    [DisplayName("Номер заказа")]
    public string Number { get; set; }
    [DisplayName("Дата заказа")]
    [DataType(DataType.Date)] 
    public DateTime Date { get; set; }
    [DisplayName("Поставщик")]
    public int ProviderId { get; set; }
    public Provider? Provider { get; set; }
}