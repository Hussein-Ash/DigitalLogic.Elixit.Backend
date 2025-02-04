using System;
using Elixir.DATA.DTOs;
using Elixir.DATA.DTOs.StoreCategory;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers;

public class StoreCategoryController : BaseController
{
    private readonly IStoreCategoryService _service;

    public StoreCategoryController(IStoreCategoryService service)
    {
        _service = service;
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Respons<StoreCategoryDto>>> GetAll([FromQuery] BaseFilter filter) => OkPaginated(await _service.GetAll(filter), filter.PageNumber, filter.PageSize);

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<StoreCategoryDto>> GetById(Guid id) => Ok(await _service.GetById(id));

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<StoreCategoryDto>> Add([FromBody] StoreCategoryForm form) => Ok(await _service.Add(form));

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<StoreCategoryDto>> Update(Guid id, [FromBody] StoreCategoryForm update) => Ok(await _service.Update(id, update));


    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<StoreCategoryDto>> Delete(Guid id) => Ok(await _service.Delete(id));



}
