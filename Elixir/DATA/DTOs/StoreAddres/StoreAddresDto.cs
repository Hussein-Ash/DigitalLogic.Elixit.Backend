using System;

namespace Elixir.DATA.DTOs.StoreAddres;

public class StoreAddresDto : BaseDto<Guid>
{
    public  string? Name { get; set; }
    public string? GovernorateName { get; set; }
    public string? CityName { get; set; }
    public string? NearestPoint { get; set; }
    public  long? Lat { get; set; }
    public  long? Lng { get; set; }
    public Guid? StoreId { get; set; }
    public string? StoreName { get; set; }
}
