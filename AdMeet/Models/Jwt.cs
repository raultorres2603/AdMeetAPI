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

    private bool ValToken(string token)
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
            return true;
        }
        catch
        {
            return false;
        }
    }

    public User DecodeToken(string token)
    {
        var tokenValidated = ValToken(token);

        if (!tokenValidated) throw new Exception("Token not validated");

        // Decode token to get user info
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(SecretKey!);
        try
        {
            var tokenDescriptor = tokenHandler.ReadJwtToken(token);
            var userId = tokenDescriptor.Claims.FirstOrDefault(c => c.Type == "Id")!.Value;
            var email = tokenDescriptor.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
            var password = tokenDescriptor.Claims.FirstOrDefault(c => c.Type == "Password")!.Value;
            var name = tokenDescriptor.Claims.FirstOrDefault(c => c.Type == "Name")?.Value;
            var lastName = tokenDescriptor.Claims.FirstOrDefault(c => c.Type == "LastName")?.Value;
            var city = tokenDescriptor.Claims.FirstOrDefault(c => c.Type == "City")?.Value;
            var country = tokenDescriptor.Claims.FirstOrDefault(c => c.Type == "Country")?.Value;
            var zipCode = tokenDescriptor.Claims.FirstOrDefault(c => c.Type == "ZipCode")?.Value;

            var u = new User(email, password);
            u.Id = userId;
            if (name != null) u.Profile.Name = name;
            if (lastName != null) u.Profile.LastName = lastName;
            if (city != null) u.Profile.City = city;
            if (country != null) u.Profile.Country = country;
            if (zipCode != null) u.Profile.ZipCode = zipCode;
            return u;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}