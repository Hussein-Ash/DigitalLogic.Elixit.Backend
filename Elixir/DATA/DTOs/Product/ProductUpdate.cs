using System;

namespace Elixir.DATA.DTOs.Product;

public class ProductUpdate
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? Features { get; set; }

    public string? DeliveryInfo { get; set; }

    public decimal Price { get; set; }
    public int Discount { get; set; }

    public int Stock { get; set; }
}
