using System;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.Order;

public class UserOrdersDto
{
    public string? StoreName { get; set; }
    public int? Rating { get; set; }
    public long? TotalPrice { get; set; }
    public OrderState? Status { get; set; }

}
