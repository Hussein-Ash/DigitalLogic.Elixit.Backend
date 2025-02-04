using System;

namespace Elixir.DATA.DTOs.ProductCategory;

public class ProductCategoryDto : BaseDto<Guid>
{
    public Guid ProductId { get; set; }
    public List<Guid>? CategoriesId { get; set; } 

}
