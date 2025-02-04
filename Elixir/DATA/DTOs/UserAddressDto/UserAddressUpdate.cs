using System;

namespace Elixir.DATA.DTOs.UserAddressDto;

public class UserAddressUpdate
{

    public string? Name { get; set; }
    public string? GovernorateName { get; set; }
    public string? CityName { get; set; }
    public string? NearestPoint { get; set; }
    public long Lat { get; set; }
    public long Lng { get; set; }


}
