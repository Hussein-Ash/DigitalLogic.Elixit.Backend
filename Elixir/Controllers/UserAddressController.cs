using Elixir.DATA.DTOs.UserAddressDto;
using Elixir.Interface;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers
{

    public class UserAddressController : BaseController
    {
        private readonly IUserAddressService _service;
        public UserAddressController(IUserAddressService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Respons<UserAddressDto>>> GetAll([FromQuery] UserAddressFilter filter) => OkPaginated(await _service.GetAll(filter), filter.PageNumber, filter.PageSize);


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<UserAddressDto>> Create([FromBody] UserAddressForm form) => Ok(await _service.Create(form));

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Respons<UserAddressDto>>> GetById(Guid id) => Ok(await _service.GetById(id));

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) => Ok(await _service.Delete(id));

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(UserAddressUpdate Update, Guid id) => Ok(await _service.Update(Update, id));



    }
}
