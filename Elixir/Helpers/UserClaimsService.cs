using System.Security.Claims;

namespace Elixir.Helpers
{


    public interface IUserClaimsService
    {
        Guid? GetUserId();
        Guid? GetStoreId();
        string? GetUserRole();

    }

    public class UserClaimsService : IUserClaimsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public Guid? GetUserId()
        {
            return Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        public Guid? GetStoreId()
        {
            return Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("StoreId")?.Value);
        }

        public string? GetUserRole()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("Role")?.Value;
        }
        public string? GetStoreRole()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("StoreRole")?.Value;
        }
    }

}