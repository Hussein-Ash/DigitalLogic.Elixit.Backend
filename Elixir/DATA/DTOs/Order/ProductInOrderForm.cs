using System;

namespace Elixir.DATA.DTOs.Order;

public class ProductInOrderForm
{
    public Guid ProductId { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public int Quantity { get; set; }

}
