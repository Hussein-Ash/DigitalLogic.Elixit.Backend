using System;

namespace Elixir.DATA.DTOs.UserAddressDto;

public class UserAddressDto : BaseDto<Guid>
{
    public Guid UserId { get; set; }

    public string? Name { get; set; }
    public string? GovernorateName { get; set; }
    public string? CityName { get; set; }
    public string? NearestPoint { get; set; }
    public long Lat { get; set; }
    public long Lng { get; set; }

}
