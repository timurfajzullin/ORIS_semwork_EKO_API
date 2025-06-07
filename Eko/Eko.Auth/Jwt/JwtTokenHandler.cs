using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Eko.Database.Entities;
using Microsoft.IdentityModel.Tokens;
namespace Eko.Auth.Jwt;

public class JwtTokenHandler : IJwtTokenHandler
{
    public async Task<string> GenerateToken(Person? person)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
            new Claim(ClaimTypes.Role, person.IsAdmin ? Policy.Admin : Policy.AdminOrUser)
        };
        
        
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecurityKey)),
            SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            JwtOptions.Issuer,
            JwtOptions.Audience,
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddMinutes(JwtOptions.ExpirationTime)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public interface IJwtTokenHandler
{
    public Task<string> GenerateToken(Person? person);
}