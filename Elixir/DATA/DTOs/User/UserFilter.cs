using System;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.User;

public class UserFilter : BaseFilter
{
    public UserRole? Role { get; set; }
}
