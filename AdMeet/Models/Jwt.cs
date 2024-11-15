using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdMeet.Inter;
using Microsoft.IdentityModel.Tokens;

namespace AdMeet.Models;

public class Jwt: IJwt
{
    public string SecretKey { get; set; } = Environment.GetEnvironmentVariable("JWT_SK")!;
    public string Issuer { get; set; } = "AdMeetI";
    public string Audience { get; set; } = "AdMeetU";
    public int ExpiresInMinutes { get; set; } = 60;

    public string GenerateToken(User u)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(SecretKey!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "AdMeet"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.NameIdentifier, u.Id),
                new Claim("Id", u.Id),
                new Claim("Email", u.Email!)
            }), Issuer = Issuer,
            Audience = Audience,
            Expires = DateTime.UtcNow.AddMinutes(ExpiresInMinutes),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}