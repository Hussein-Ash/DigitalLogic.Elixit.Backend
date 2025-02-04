using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elixir.Entities;

public class ProductComment : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public AppUser? User { get; set; }

    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    public int Likes { get; set; } 

    public required string Content { get; set; }

    public Guid? ParentId { get; set; }
    public List<ProductComment>? ReplyComments { get; set; }

    public int Replies { get; set; }




    public void Reply(int increase,int decrease)
    {
        Replies = Replies + increase - decrease;
    }

    public void Like(int increase,int decrease)
    {
        Likes = Likes + increase - decrease;

    }

}
