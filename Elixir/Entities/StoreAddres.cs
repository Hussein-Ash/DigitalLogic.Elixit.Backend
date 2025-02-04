using System;

namespace Elixir.Entities;

public class StoreAddres : BaseEntity<Guid>
{
    public required string Name { get; set; }
    public string? GovernorateName { get; set; }
    public string? CityName { get; set; }
    public string? NearestPoint { get; set; }
    public required long Lat { get; set; }
    public required long Lng { get; set; }
    public Guid StoreId { get; set; }
    public Store? Store { get; set; }

}
