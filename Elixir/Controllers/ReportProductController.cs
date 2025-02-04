using System;
using Elixir.DATA.DTOs.ReportProducts;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers
{

    public class ReportProductController : BaseController
    {
         private readonly IReportProductService _service;
        public ReportProductController(IReportProductService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Respons<ReportProductDto>>> GetAll([FromQuery] ReportProductFilter filter) => OkPaginated(await _service.GetAll(filter), filter.PageNumber, filter.PageSize);


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ReportProductDto>> Create([FromBody] ReportProductForm form) => Ok(await _service.Add(form,Id));

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) => Ok(await _service.Delete(id));

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(ReportProductUpdate Update, Guid id) => Ok(await _service.AdminResponse(id,Update, Id));



        
    }
}
