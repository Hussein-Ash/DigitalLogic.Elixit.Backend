using System;
using System.ComponentModel.DataAnnotations;

namespace Elixir.Entities;

public class Order : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public Guid UserAddressId { get; set; }
    public UserAddress? UserAddress { get; set; }
    public DeliveryType DeliveryType { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public List<ProductInOrder>? Products { get; set; }

    [Range(1, 5)]
    public int? Rating { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderState Status { get; set; }

    

}

public enum OrderState
{
    pending,
    accepted,
    inProgress,
    inStore,
    inTheWay,
    delivered,
    canceled
}

public enum DeliveryType
{
    regular,
    fast
}