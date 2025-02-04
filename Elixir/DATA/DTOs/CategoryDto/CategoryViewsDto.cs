using System;

namespace Elixir.DATA.DTOs.CategoryDto;

public class CategoryViewsDto : BaseDto<Guid>
{
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public long Videos { get; set; }
    public bool? IsActive { get; set; }

}
