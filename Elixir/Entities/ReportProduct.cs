using System;

namespace Elixir.Entities;

public class ReportProduct : BaseEntity<Guid>
{

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public Guid? AdminId { get; set; }
    public AppUser? Admin { get; set; }
    public ReportStatus Status { get; set; }
    public string? Reason { get; set; }
    public string? AdminNote { get; set; }

}


public enum ReportStatus
{
    pending,
    accepted,
    rejected
}