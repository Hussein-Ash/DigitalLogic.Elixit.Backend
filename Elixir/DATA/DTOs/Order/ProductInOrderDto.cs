using System;

namespace Elixir.DATA.DTOs.Order;

public class ProductInOrderDto : BaseDto<Guid>
{
    public Guid? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? StoreName { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public int? Quantity { get; set; }

}