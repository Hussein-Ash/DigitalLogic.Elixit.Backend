using System;

namespace Elixir.DATA.DTOs.UserSave;

public class UserSaveDto:BaseDto<Guid>
{
    public Guid UserId { get; set; }
    public Guid? ProductId { get; set; }
}

