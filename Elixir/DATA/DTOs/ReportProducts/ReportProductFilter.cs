using System;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.ReportProducts;

public class ReportProductFilter : BaseFilter
{
    public ReportStatus? Status { get; set; }
}
