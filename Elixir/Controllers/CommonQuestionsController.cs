using Elixir.DATA.DTOs.CommonQuestions;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers
{

    public class CommonQuestionsController : BaseController
    {
        private readonly ICommonQuestionService _service;
        public CommonQuestionsController(ICommonQuestionService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Respons<CommonQuestionDto>>> GetAll([FromQuery] CommonQuestionFilter filter) => OkPaginated(await _service.GetAll(filter), filter.PageNumber, filter.PageSize);

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CommonQuestionDto>> Create([FromBody] CommonQuestionForm form) => Ok(await _service.Add(form));

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Respons<CommonQuestionDto>>> GetById(Guid id) => Ok(await _service.GetById(id));

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) => Ok(await _service.Delete(id));

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(CommonQuestionUpdate update ,Guid id) => Ok(await _service.Update(id,update));
    }
}
