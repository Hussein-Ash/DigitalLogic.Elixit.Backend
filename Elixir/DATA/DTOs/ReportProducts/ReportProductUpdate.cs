using System;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.ReportProducts;

public class ReportProductUpdate
{
    public ReportStatus Status { get; set; }
    public string? AdminNote { get; set; }
}
