using System;

namespace Elixir.Entities;

public class Like : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid? CommentId { get; set; }
    public ProductComment? Comment { get; set; }

    public LikeType Type { get; set; }



}

public enum LikeType
{
    Comment,
    Product,
}