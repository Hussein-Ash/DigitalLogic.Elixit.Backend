using System;
using Elixir.DATA.DTOs;
using Elixir.DATA.DTOs.UserStore;
using Elixir.Entities;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers;

public class UserStoresController : BaseController
{
    private readonly IUserStoreService _service;

    public UserStoresController(IUserStoreService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Respons<UserStoreDto>>> GetAll([FromQuery] UserStoreFilter filter) => OkPaginated(await _service.GetAll(filter), filter.PageNumber, filter.PageSize);


    [Authorize]
    [HttpPost]
    public async Task<ActionResult<UserStoreDto>> Add([FromBody] UserStoreForm form) => Ok(await _service.Add(form));

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<UserStoreDto>> Update(Guid id, [FromBody] UserStoreForm update) => Ok(await _service.Update(id, update));
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<UserStoreDto>> Delete(Guid id) => Ok(await _service.Delete(id));

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserStoreDto>> GetById(Guid id) => Ok(await _service.GetById(id));

}
