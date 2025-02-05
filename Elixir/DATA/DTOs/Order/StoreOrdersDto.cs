using System;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.Order;

public class StoreOrdersDto : BaseDto<Guid>
{
    public string? CustomerName { get; set; }
    public string? ProductName { get; set; }
    public string? StoreName { get; set; }
    public OrderState OrderState { get; set; }
    public decimal TotalPrice { get; set; }

}
