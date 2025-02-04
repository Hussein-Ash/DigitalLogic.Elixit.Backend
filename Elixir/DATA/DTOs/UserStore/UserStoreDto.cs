using System;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.UserStore;

public class UserStoreDto : BaseDto<Guid>
{
    public Guid? StoreId { get; set; }
    public Guid? UserId { get; set; }
    public EmployeeRole? Role { get; set; }


}
