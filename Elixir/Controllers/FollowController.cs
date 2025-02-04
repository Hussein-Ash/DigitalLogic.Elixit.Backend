using Elixir.DATA.DTOs.Follow;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers
{
    public class FollowController : BaseController
    {
        private readonly IFollowService _service;

        public FollowController(IFollowService service)
        {
            _service = service;
        }


        [Authorize]
        [HttpGet("followers")]
        public async Task<ActionResult<Respons<FollowDto>>> GetAllFollowers([FromQuery] FollowFilter filter) => OkPaginated(await _service.GetAllFollowers(filter, Id), filter.PageNumber, filter.PageSize);
        [Authorize]
        [HttpGet("following")]
        public async Task<ActionResult<Respons<FollowDto>>> GetAllFollowing([FromQuery] FollowFilter filter) => OkPaginated(await _service.GetAllFollowings(filter, Id), filter.PageNumber, filter.PageSize);

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<FollowDto>> Follow([FromBody] FollowForm form) => Ok(await _service.AddFollow(form, Id));

        [Authorize]
        [HttpDelete("follower/{id}")]
        public async Task<ActionResult> DeleteFollower(Guid id) => Ok(await _service.RemoveFollower(id, Id));

        [Authorize]
        [HttpDelete("following/{id}")]
        public async Task<ActionResult> DeleteFollowing(Guid id) => Ok(await _service.RemoveFollowing(id, Id));



    }
}
