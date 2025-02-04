using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elixir.Entities;

public class Product : BaseEntity<Guid>
{
    public Guid StoreId { get; set; }
    public Store? Store { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Features { get; set; }

    public string? DeliveryInfo { get; set; }

    public decimal Price { get; set; }
    public int Discount { get; set; }

    public int Stock { get; set; }  
    public string? Thumbnail { get; set; }
    public string? Url1080 { get; set; }
    public string? Url720 { get; set; }
    public string? Url480 { get; set; }
    public int Likes { get; set; }
    public int Comments { get; set; } = 0;
    public string? Note { get; set; }


    public void Comment(int increase,int decrease)
    {
        Comments = Comments + increase - decrease;
    }
    public void Like(int increase,int decrease)
    {
        Likes = Likes + increase - decrease;

    }

    
    




 

}
