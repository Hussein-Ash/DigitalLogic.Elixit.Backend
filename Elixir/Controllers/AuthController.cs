using Elixir.DATA.DTOs.User;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/api/Login")]
        public async Task<ActionResult> Login(LoginForm loginForm) => Ok(await _userService.Login(loginForm));
        [HttpPost("/api/Register")]
        public async Task<ActionResult> Register(RegisterForm registerForm) => Ok(await _userService.Register(registerForm));

        [Authorize]
        [HttpGet("/api/User/{id}")]
        public async Task<ActionResult> GetUser(Guid id) => Ok(await _userService.GetUserById(id));

        [Authorize]
        [HttpGet("/api/Users")]
        public async Task<ActionResult<Respons<UsersDto>>> GetUsers([FromQuery] UserFilter filter) => OkPaginated(await _userService.GetAll(filter), filter.PageNumber, filter.PageSize);

        [Authorize]
        [HttpPut("/api/User/{id}")]
        public async Task<ActionResult> UpdateUser(UpdateUserForm updateUserForm, Guid id) => Ok(await _userService.UpdateUser(updateUserForm, id));

        [HttpPost("/api/add-user")]
        public async Task<ActionResult> AddAmin(AdminForm loginForm) => Ok(await _userService.AddAdmin(loginForm,Id));
        

        [Authorize]
        [HttpDelete("/api/User/Delete-Account")]
        public async Task<ActionResult> DeleteUser(Guid id) => Ok(await _userService.DeleteUser(Id));

        [Authorize]
        [HttpPut("/api/User/Categories")]
        public async Task<ActionResult> AddUserCategory(UserCategoryForm form) => Ok(await _userService.AddUserCategory(form, Id));

        [Authorize]
        [HttpPost("/api/SwitchToStore/{StoreId}")]
        public async Task<ActionResult> SwitchToStore(Guid StoreId) => Ok(await _userService.SwitchToStore(StoreId, Id));

        [Authorize]
        [HttpPut("/api/User/change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordForm form) => Ok(await _userService.UserChangePassword(form, Id));

    }
}