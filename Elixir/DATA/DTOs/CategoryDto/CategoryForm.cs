using System;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.CategoryDto;

public class CategoryForm
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Img { get; set; }

}
