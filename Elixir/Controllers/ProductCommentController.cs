using System;
using Elixir.DATA.DTOs.ProductComment;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers;

public class ProductCommentController : BaseController
{
    private readonly IProductCommentService _service;

    public ProductCommentController(IProductCommentService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Respons<ProductCommentDto>>> GetAll([FromQuery] ProductCommentFilter filter) => OkPaginated(await _service.GetAll(filter), filter.PageNumber, filter.PageSize);

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductCommentDto>> GetById(Guid id) => Ok(await _service.GetById(id));

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ProductCommentDto>> Add([FromBody] ProductCommentForm form) => Ok(await _service.Add(form, Id));

    [Authorize]
    [HttpPost("reply")]
    public async Task<ActionResult<ProductCommentDto>> Reply([FromBody] ProductReplyForm form) => Ok(await _service.Reply(form, Id));

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductCommentDto>> Update(Guid id, [FromBody] ProductCommentUpdate update) => Ok(await _service.Update(id, update, Id));

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ProductCommentDto>> Delete(Guid id) => Ok(await _service.Delete(id, Id));



}
