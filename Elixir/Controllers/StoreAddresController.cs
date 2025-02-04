using System;
using Elixir.DATA.DTOs.StoreAddres;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers;

public class StoreAddresController : BaseController
{
    private readonly IStoreAddresService _service;

    public StoreAddresController(IStoreAddresService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Respons<StoreAddresDto>>> GetAll([FromQuery] StoreAddresFilter filter) => OkPaginated(await _service.GetAll(filter), filter.PageNumber, filter.PageSize);

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<StoreAddresDto>> GetById(Guid id) => Ok(await _service.GetById(id));


    [Authorize]
    [HttpPost]
    public async Task<ActionResult<StoreAddresDto>> Add([FromBody] StoreAddresForm form) => Ok(await _service.Add(form));


    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<StoreAddresDto>> Update(Guid id, [FromBody] StoreAddresUpdate update) => Ok(await _service.Update(id, update));

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<StoreAddresDto>> Delete(Guid id) => Ok(await _service.Delete(id));



}
