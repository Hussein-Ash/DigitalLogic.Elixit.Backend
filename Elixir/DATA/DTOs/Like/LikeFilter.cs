using System;

namespace Elixir.DATA.DTOs.Like;

public class LikeFilter : BaseFilter
{
    public Guid? CommentId { get; set; }

    public Guid? ProductId { get; set; }

}
