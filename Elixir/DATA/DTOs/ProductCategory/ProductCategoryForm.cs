using System;

namespace Elixir.DATA.DTOs.ProductCategory;

public class ProductCategoryForm
{
    public required Guid ProductId { get; set; }
    public required List<Guid> CategoriesId { get; set; } 

}
