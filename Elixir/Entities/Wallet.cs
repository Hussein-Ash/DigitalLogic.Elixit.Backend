using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elixir.Entities;

public class Wallet: BaseEntity<Guid>
{
    public Guid StoreId { get; set; }
    [ForeignKey(nameof(StoreId))]
    public Store? Store { get; set; }
    public decimal Amount { get; set; } = 0;


}
