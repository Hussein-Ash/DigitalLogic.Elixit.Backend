using System;

namespace Elixir.Entities;

public class Follow : BaseEntity<Guid>
{
    public Guid? UserId { get; set; }
    public AppUser? User { get; set; }
    public Guid? StoreId { get; set; }
    public Store? Store { get; set; }
    public Guid? FollowedStoreId { get; set; }
    public Store? FollowedStore { get; set; }

    public FollowerType Follower { get; set; }



}

public enum FollowerType
{
    User,
    Store,
}
