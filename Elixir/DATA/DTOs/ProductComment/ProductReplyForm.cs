using System;

namespace Elixir.DATA.DTOs.ProductComment;

public class ProductReplyForm
{
    public Guid CommentId { get; set; }
    public required string Content { get; set; }

}
