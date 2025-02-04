using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elixir.Entities;

public class Store : BaseEntity<Guid>
{
    public required string Name { get; set; }
    public string? NickName { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public int Followers { get; set; }
    public int Followings { get; set; }
    public int Posts { get; set; }
    public Guid OwnerId { get; set; }
    public AppUser? Owner { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public List<StoreAddres>? Address { get; set; }
    [MinLength(1)]
    public required List<string> Imgs { get; set; }
    public List<UserStore>? Employees { get; set; }
    public List<Product>? Products { get; set; }

    public Guid WalletId { get; set; }
    [ForeignKey(nameof(WalletId))]
    public Wallet? Wallet { get; set; }



    public void Follower(int increase, int decrease)
    {
        Followers = Followers + increase - decrease;

    }
    public void Following(int increase, int decrease)
    {
        Followings = Followings + increase - decrease;

    }
    public void Post(int increase, int decrease)
    {
        Posts = Posts + increase - decrease;

    }

}


public enum PaymentMethod
{
    Cash = 1,
    CreditCard = 2,
}