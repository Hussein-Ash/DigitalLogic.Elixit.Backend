using System;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.CategoryDto;

public class CategoryUpdate
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public string? Img { get; set; }

}
