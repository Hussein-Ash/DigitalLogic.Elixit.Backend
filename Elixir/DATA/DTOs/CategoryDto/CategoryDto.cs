using System;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.CategoryDto;

public class CategoryDto : BaseDto<Guid>
{
    public string? Name { get; set; }
    public string? Img { get; set; }
    public string? Description { get; set; }


    public List<CategoryDto>? SubCategory { get; set; }

}
