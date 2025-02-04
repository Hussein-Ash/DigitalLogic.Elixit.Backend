using System;

namespace Elixir.DATA.DTOs.ProductComment;

public class ProductCommentForm
{
    public Guid ProductId { get; set; }
    public required string Content { get; set; }
    

}
