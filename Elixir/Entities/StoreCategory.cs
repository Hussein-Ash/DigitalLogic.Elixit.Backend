using System;

namespace Elixir.Entities;

public class StoreCategory:BaseEntity<Guid>
{
    public Guid StoreId { get; set; }
    public Store? Store { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

}
