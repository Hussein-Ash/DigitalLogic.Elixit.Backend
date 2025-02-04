using System;

namespace Elixir.DATA.DTOs.Product;

public class ProductDto : BaseDto<Guid>
{
    public string? StoreName { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Features { get; set; }

    public string? DeliveryInfo { get; set; }

    public decimal Price { get; set; }
    public int Discount { get; set; }

    public int Stock { get; set; }  

    public string? Url1080 { get; set; }
    public string? Url720 { get; set; }
    public string? Url480 { get; set; }

    public int Likes { get; set; }
    public int Comments { get; set; }

}
