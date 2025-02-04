using System;

namespace Elixir.DATA.DTOs.User;

public class UsersDto : BaseDto<Guid>
{
    public string? FullName { get; set; }
    public string? UserName { get; set; }
    public string? Role { get; set; }
    public bool Active { get; set; }

}
