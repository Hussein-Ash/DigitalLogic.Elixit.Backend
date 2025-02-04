using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using e_parliament.Interface;
using Elixir.DATA.DTOs.User;
using Elixir.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Elixir.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)  
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(UserDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                // add role Claim
                new Claim("id", user.Id.ToString()),
                new Claim("Role", user.Role),
                new Claim("Active", user.Active.ToString()),
                new Claim("StoreId", user.StoreId.ToString()),
                new Claim("StoreRole", user.StoreRole.ToString()),
                // new Claim("Categories", user.CategoriesId.ToString())

                // new Claim(JwtRegisteredClaimNames., user.Email.ToString()),
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }   

    }
}