using System;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.Store;

public class StoreUpdate
{
    public string? Name { get; set; }
    public string? NickName { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public List<string>? Imgs { get; set; }

}
