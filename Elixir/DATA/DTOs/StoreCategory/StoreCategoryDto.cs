using System;

namespace Elixir.DATA.DTOs.StoreCategory;

public class StoreCategoryDto:BaseDto<Guid>
{
    public Guid? StoreId { get; set; }
    public string? StoreName { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }


}
