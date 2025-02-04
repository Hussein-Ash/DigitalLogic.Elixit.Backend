using System;

namespace Elixir.Entities;

public class UserCategory : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
}
