using System;

namespace Elixir.Entities;

public class CreditCard : BaseEntity<Guid>
{
    public required string CardNumber { get; set; }
    public required int SecretNumber { get; set; }
    public required DateTime CardDate { get; set; }
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }

}
