using System;
using Elixir.DATA.DTOs;
using Elixir.DATA.DTOs.Order;
using Elixir.Extensions.StoreAuthoeization;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers;

public class OrderController : BaseController
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("store-orders")]
    public async Task<ActionResult<Respons<StoreOrdersDto>>> GetAllStoreOrders([FromQuery] OrderFilter filter) => OkPaginated(await _service.GetAllStoreOrders(filter, Id), filter.PageNumber, filter.PageSize);

    [Authorize]
    [HttpGet("user-orders")]
    public async Task<ActionResult<Respons<UserOrdersDto>>> GetAllUserOrders([FromQuery] OrderFilter filter) => OkPaginated(await _service.GetAllUserOrders(filter, Id), filter.PageNumber, filter.PageSize);

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Respons<UserOrdersDto>>> GetAll([FromQuery] OrderFilter filter) => OkPaginated(await _service.GetAll(filter), filter.PageNumber, filter.PageSize);


    [Authorize]
    [HttpPost]
    public async Task<ActionResult<OrderDto>> Add([FromBody] OrderForm form) => Ok(await _service.Add(form, Id));

    [StoreAuthorize]
    [HttpPut("store/{id}")]
    public async Task<ActionResult<OrderDto>> Update(Guid id, [FromBody] OrderUpdate update) => Ok(await _service.Update(id, update));

    [Authorize(Roles = "User,SuperAdmin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<OrderDto>> UserUpdate(Guid id, [FromBody] OrderUpdate update) => Ok(await _service.UserUpdate(id, update));

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<OrderDto>> Delete(Guid id) => Ok(await _service.Delete(id, Id));

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid id) => Ok(await _service.GetById(id));



}
