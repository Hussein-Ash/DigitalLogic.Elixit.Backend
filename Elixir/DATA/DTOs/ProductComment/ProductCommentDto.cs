using System;

namespace Elixir.DATA.DTOs.ProductComment;

public class ProductCommentDto : BaseDto<Guid>
{
    public Guid? ProductId { get; set; }
    public Guid? UserId { get; set; }
    public string? Content { get; set; }
    public string? UserName { get; set; }
    public string? UserImg { get; set; }
    public List<ProductCommentDto>? ReplyComments { get; set; }


    public int Replies { get; set; }


}
