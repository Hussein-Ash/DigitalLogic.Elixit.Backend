using Elixir.DATA.DTOs.Like;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers
{

    public class LikeController : BaseController
    {

        private readonly ILikeService _service;

        public LikeController(ILikeService service)
        {
            _service = service;
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Respons<LikeDto>>> GetAllFollowers([FromQuery] LikeFilter filter) => OkPaginated(await _service.GetAllLikes(filter), filter.PageNumber, filter.PageSize);

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<LikeDto>> Follow([FromBody] LikeForm form) => Ok(await _service.AddLike(form, Id));

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFollower(Guid id) => Ok(await _service.RemoveLike(id));

    }
}
