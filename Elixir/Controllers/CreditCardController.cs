using Elixir.DATA.DTOs.CreditCard;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers
{

    public class CreditCardController : BaseController
    {
        private readonly ICreditCardService _service;

        public CreditCardController(ICreditCardService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Respons<CreditCardDto>>> GetAll([FromQuery] CreditCardFilter filter) => OkPaginated(await _service.GetAll(filter,Id), filter.PageNumber, filter.PageSize);

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CreditCardController>> GetById(Guid id) => Ok(await _service.GetById(id,Id));

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CreditCardController>> Add([FromBody] CreditCardForm form) => Ok(await _service.Add(form, Id));

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<CreditCardController>> Update(Guid id, [FromBody] CreditCardUpdate update) => Ok(await _service.Update(id, update,Id));

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CreditCardController>> Delete(Guid id) => Ok(await _service.Delete(id, Id));
    }
}
