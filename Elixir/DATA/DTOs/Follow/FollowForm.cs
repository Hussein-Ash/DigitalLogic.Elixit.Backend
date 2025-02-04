using System;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.Follow;

public class FollowForm
{
    public Guid? FollowingId { get; set; }
    public FollowerType Follower { get; set; }

}
