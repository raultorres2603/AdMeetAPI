using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdMeet.Inter;
using Microsoft.IdentityModel.Tokens;

namespace AdMeet.Models;

public class Jwt(ILogger<IJwt> logger) : IJwt
{
    public string SecretKey { get; set; } = Environment.GetEnvironmentVariable("JWT_SK")!;
    public string Issuer { get; set; } = Environment.GetEnvironmentVariable("I")!;
    public string Audience { get; set; } = Environment.GetEnvironmentVariable("U")!;
    public int ExpiresInMinutes { get; set; } = 60;

    public string GenerateToken(User u)
    {
        logger.LogInformation("Generating token");
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
                new Claim("ZipCode", u.Profile.ZipCode),
                new Claim("Gender", u.Profile.Gender),
                new Claim("Birthday", u.Profile.Birthday.ToString()),
                new Claim("Preferences", u.Profile.Preferences),
                new Claim("IsAdmin", u.IsAdmin.ToString())
            }),
            Issuer = Issuer,
            Audience = Audience,
            Expires = DateTime.UtcNow.AddMinutes(ExpiresInMinutes),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        logger.LogInformation("Done generating token");
        return tokenHandler.WriteToken(token);
    }

    public User DecodeToken(string token)
    {
        logger.LogInformation("Decoding token");
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
            var gender = tokenDescriptor.Claims.FirstOrDefault(c => c.Type == "Gender")?.Value;
            var birthday = tokenDescriptor.Claims.FirstOrDefault(c => c.Type == "Birthday")?.Value;
            var preferences = tokenDescriptor.Claims.FirstOrDefault(c => c.Type == "Preferences")?.Value;
            var isAdmin = tokenDescriptor.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;

            logger.LogInformation("Done decoding token");
            var u = new User(email, password);
            u.Id = userId;
            if (name != null) u.Profile.Name = name;
            if (lastName != null) u.Profile.LastName = lastName;
            if (city != null) u.Profile.City = city;
            if (country != null) u.Profile.Country = country;
            if (zipCode != null) u.Profile.ZipCode = zipCode;
            if (gender != null) u.Profile.Gender = gender;
            if (birthday != null) u.Profile.Birthday = DateOnly.Parse(birthday);
            if (preferences != null) u.Profile.Preferences = preferences;
            if (isAdmin != null) u.IsAdmin = bool.Parse(isAdmin);
            return u;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error decoding token");
            throw new Exception(e.Message);
        }
    }

    private bool ValToken(string token)
    {
        logger.LogInformation("Validating token");
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
            logger.LogInformation("Done validating token");
            return true;
        }
        catch
        {
            logger.LogError("Error validating token");
            return false;
        }
    }
}