using System;
using Elixir.DATA.DTOs.Order;
using Elixir.Entities;

namespace Elixir.DATA.DTOs;

public class OrderForm
{
    public Guid UserAddressId { get; set; }
    public DeliveryType DeliveryType { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public List<ProductInOrder>? ProductInOrders { get; set; }
    public int? Rating { get; set; }
}
