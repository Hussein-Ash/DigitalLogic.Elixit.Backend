using System;

namespace Elixir.DATA.DTOs.ProductComment;

public class ProductCommentFilter : BaseFilter
{
    public Guid? ProjectId { get; set; }

}
