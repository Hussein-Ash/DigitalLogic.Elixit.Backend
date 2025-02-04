using System;

namespace Elixir.Entities;

public class CommonQuestion : BaseEntity<Guid>
{
    public string? Title { get; set; }

    public string? Description { get; set; }

}
