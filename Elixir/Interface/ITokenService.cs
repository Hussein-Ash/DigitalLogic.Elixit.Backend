using Elixir.DATA.DTOs.User;
using Elixir.Entities;

namespace e_parliament.Interface
{
    public interface ITokenService
    {
        string CreateToken(UserDto user);
    }
}