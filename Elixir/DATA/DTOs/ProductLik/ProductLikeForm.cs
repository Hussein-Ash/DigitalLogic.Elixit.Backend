using System;

namespace Elixir.DATA.DTOs.ProductLik;

public class ProductLikeForm
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
}
