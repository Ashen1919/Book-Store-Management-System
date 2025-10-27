using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RathnaBookStore.API.Repositories.Auth_Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CreateJwtToken(IdentityUser user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    claims,
                    expires : DateTime.Now.AddMinutes(2880),
                    signingCredentials: credential
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
