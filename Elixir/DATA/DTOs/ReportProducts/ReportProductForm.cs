using System;

namespace Elixir.DATA.DTOs.ReportProducts;

public class ReportProductForm
{
    public Guid ProductId { get; set; }
    public required string Reason { get; set; }

}
