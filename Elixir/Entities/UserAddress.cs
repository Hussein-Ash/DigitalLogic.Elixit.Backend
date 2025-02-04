using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elixir.Entities;

public class UserAddress : BaseEntity<Guid>
{
    public string? Name { get; set; }
    public string? GovernorateName { get; set; }
    public string? CityName { get; set; }
    public string? NearestPoint { get; set; }
    public long Lat { get; set; }
    public long Lng { get; set; }

    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public AppUser? User { get; set; }



}
