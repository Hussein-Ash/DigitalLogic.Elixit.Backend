using System;
using System.ComponentModel.DataAnnotations;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.Store;

public class StoreDto : BaseDto<Guid>
{
    public string? Name { get; set; }
    public string? NickName { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public int Followers { get; set; }
    public int Followings { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public List<Entities.StoreAddres>? Address { get; set; }

    public List<string>? Imgs { get; set; }
    public List<Entities.UserStore>? Employees { get; set; }

}
