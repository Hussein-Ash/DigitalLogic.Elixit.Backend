using System;

namespace Elixir.DATA.DTOs.CreditCard;

public class CreditCardDto : BaseDto<Guid>
{
    public string? CardNumber { get; set; }
    public int? SecretNumber { get; set; }
    public DateTime? CardDate { get; set; }
    public Guid? UserId { get; set; }
}
