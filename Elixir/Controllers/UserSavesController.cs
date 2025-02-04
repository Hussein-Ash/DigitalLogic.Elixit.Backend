using System;
using Elixir.DATA.DTOs.UserSave;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers;

public class UserSavesController : BaseController
{
    private readonly IUserSaveService _service;

    public UserSavesController(IUserSaveService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Respons<UserSaveDto>>> GetAll([FromQuery] UserSaveFilter filter) => OkPaginated(await _service.GetAll(filter), filter.PageNumber, filter.PageSize);

    [Authorize]
    [HttpGet("{productId}")]
    public async Task<ActionResult<UserSaveDto>> GetById(Guid id) => Ok(await _service.GetById(id));

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<UserSaveDto>> Add([FromBody] UserSaveForm form) => Ok(await _service.Add(form));

    [Authorize]
    [HttpDelete("{productId}")]
    public async Task<ActionResult<UserSaveDto>> Delete(Guid productId) => Ok(await _service.Delete(productId));


}
