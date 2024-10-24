using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AdMeet.Models;

public class Jwt
{
    public string? SecretKey { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpiresInMinutes { get; set; }
    
    public string GenerateToken(User u)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key =Encoding.UTF8.GetBytes(this.SecretKey!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "AdMeet"),
                new Claim("Id", u.Id),
                new Claim("Name", u.Name!),
                new Claim("Email", u.Email!)
                
            }),
            Expires = DateTime.UtcNow.AddMinutes(this.ExpiresInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}