using System;

namespace Elixir.DATA.DTOs.Like;

public class LikeDto : BaseDto<Guid>
{
    public Guid? UserId { get; set; }
    public string? Name { get; set; }
    public string? UserName { get; set; }
    public string? Img { get; set; }
    public Guid? LikedId { get; set; }
    public string? Type { get; set; }

}
