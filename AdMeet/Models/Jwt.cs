using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdMeet.Inter;
using Microsoft.IdentityModel.Tokens;

namespace AdMeet.Models;

public class Jwt : IJwt
{
    public string SecretKey { get; set; } = Environment.GetEnvironmentVariable("JWT_SK")!;
    public string Issuer { get; set; } = Environment.GetEnvironmentVariable("I")!;
    public string Audience { get; set; } = Environment.GetEnvironmentVariable("U")!;
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
                new Claim("Email", u.Email!),
                new Claim("Password", u.Password!),
                new Claim("Name", u.Profile.Name),
                new Claim("LastName", u.Profile.LastName),
                new Claim("City", u.Profile.City),
                new Claim("Country", u.Profile.Country),
                new Claim("ZipCode", u.Profile.ZipCode)
            }),
            Issuer = Issuer,
            Audience = Audience,
            Expires = DateTime.UtcNow.AddMinutes(ExpiresInMinutes),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public object ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(SecretKey!);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var user = new
            {
                Id = jwtToken.Claims.First(x => x.Type == "Id").Value,
                Email = jwtToken.Claims.First(x => x.Type == "Email").Value,
                Password = jwtToken.Claims.First(x => x.Type == "Password").Value,
                Name = jwtToken.Claims.First(x => x.Type == "Name").Value,
                LastName = jwtToken.Claims.First(x => x.Type == "LastName").Value,
                City = jwtToken.Claims.First(x => x.Type == "City").Value,
                Country = jwtToken.Claims.First(x => x.Type == "Country").Value,
                ZipCode = jwtToken.Claims.First(x => x.Type == "ZipCode").Value
            };
            return user;
        }
        catch
        {
            throw new Exception("Invalid token");
        }
    }
}