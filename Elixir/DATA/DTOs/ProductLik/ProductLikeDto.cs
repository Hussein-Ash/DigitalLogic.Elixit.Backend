using System;

namespace Elixir.DATA.DTOs.ProductLik;

public class ProductLikeDto : BaseDto<Guid>
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }

}
