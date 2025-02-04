using System;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.UserStore;

public class UserStoreForm
{
    public Guid UserId { get; set; }
    public EmployeeRole Role { get; set; }

}
