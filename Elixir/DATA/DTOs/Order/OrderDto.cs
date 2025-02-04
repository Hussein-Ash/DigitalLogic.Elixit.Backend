using Elixir.DATA.DTOs.Order;
using Elixir.Entities;

namespace Elixir.DATA.DTOs;

public class OrderDto : BaseDto<Guid>
{
    public Guid? UserAddressId { get; set; }
    public UserAddressDto.UserAddressDto? UserAddress { get; set; }
    public DeliveryType? DeliveryType { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public List<ProductInOrderDto>? Products { get; set; }

    public int? Rating { get; set; }
    public long? TotalPrice { get; set; }
    public OrderState? Status { get; set; }

}
