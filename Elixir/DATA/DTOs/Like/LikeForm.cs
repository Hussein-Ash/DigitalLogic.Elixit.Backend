using System;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.Like;

public class LikeForm
{
    public Guid? LikedId { get; set; }
    public LikeType Type { get; set; }

}
