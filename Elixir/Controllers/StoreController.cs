using Elixir.DATA.DTOs.Store;
using Elixir.Entities;
using Elixir.Extensions.StoreAuthoeization;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers;

public class StoreController : BaseController
{
    private readonly IStoreService _service;

    public StoreController(IStoreService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Respons<StoreDto>>> GetAll([FromQuery] StoreFilter filter) => OkPaginated(await _service.GetAll(filter), filter.PageNumber, filter.PageSize);


    [Authorize]
    [HttpGet("store/{id}")]
    public async Task<ActionResult<Respons<Wallet>>> GetWallet(Guid id) => Ok(await _service.GetWallet(id));


    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<StoreDto>> GetById(Guid id) => Ok(await _service.GetById(id));

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<StoreDto>> Add([FromBody] StoreForm form) => Ok(await _service.Add(form, Id));

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<StoreDto>> Update(Guid id, [FromBody] StoreUpdate update) => Ok(await _service.Update(id, update, Id));

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<StoreDto>> Delete(Guid id) => Ok(await _service.Delete(id, Id));



}
