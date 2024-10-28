using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AdMeet.Models;

public interface IJwt
{
    public string? SecretKey { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpiresInMinutes { get; set; }
    public string GenerateToken(IUser u);
}

public class Jwt: IJwt
{
    public string? SecretKey { get; set; } = Environment.GetEnvironmentVariable("JWT_SK");
    public string? Issuer { get; set; } = "AdMeetI";
    public string? Audience { get; set; } = "AdMeetU";
    public int ExpiresInMinutes { get; set; } = 60;

    public string GenerateToken(IUser u)
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