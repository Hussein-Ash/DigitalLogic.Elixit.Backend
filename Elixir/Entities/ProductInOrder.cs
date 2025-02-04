using System;

namespace Elixir.Entities;

public class ProductInOrder : BaseEntity<Guid>
{
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public int Quantity { get; set; }

}
