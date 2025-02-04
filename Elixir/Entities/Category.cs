using System;

namespace Elixir.Entities;

public class Category : BaseEntity<Guid>
{
    public string? Name { get; set; }
    public string? Img { get; set; }
    public string? Description { get; set; }
    public Guid? ParentId { get; set; }
    public Category? ParentCategory { get; set; }
    public bool IsActive { get; set; }
    // public List<UserCategory>? Users { get; set; }
    public List<Category> SubCategory { get; set; } = [];

}
