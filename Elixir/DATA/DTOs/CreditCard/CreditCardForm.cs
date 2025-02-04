using System;

namespace Elixir.DATA.DTOs.CreditCard;

public class CreditCardForm
{
    public string? CardNumber { get; set; }
    public int? SecretNumber { get; set; }
    public DateTime? CardDate { get; set; }

}
