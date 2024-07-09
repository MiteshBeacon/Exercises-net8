using API.Entity;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(AppUser appUser)
        {
            var tokenKey = config["TokenKey"] ?? throw new Exception("Can not access tokenken from appseting");
            if (tokenKey.Length < 64) throw new Exception("Your tokenkey need to be longer");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,appUser.UserName)
            };

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = cred
            };
            var tokenhandeler = new JwtSecurityTokenHandler();
            var token= tokenhandeler.CreateToken(tokendescriptor);
            return tokenhandeler.WriteToken(token);
        }
    }
}
