using System;

namespace Elixir.DATA.DTOs.Follow;

public class FollowDto : BaseDto<Guid>
{
    public Guid? UserId { get; set; }
    public string? Name { get; set; }
    public string? UserName { get; set; }
    public string? Img { get; set; }
    public string? Type { get; set; }

}
