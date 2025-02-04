using System;
using System.ComponentModel.DataAnnotations;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.Store;

public class StoreForm
{

    public required string Name { get; set; }
    public string? NickName { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    [MinLength(1)]
    public required List<string> Imgs { get; set; }

}
