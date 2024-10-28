using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace AdMeet.Attributes;

public class JwtAuth : Attribute, IAuthorizationFilter
{
    private readonly string _secretKey = Environment.GetEnvironmentVariable("JWT_SK")!;
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (authorizationHeader == null || !authorizationHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = authorizationHeader["Bearer ".Length..].Trim();

        if (string.IsNullOrEmpty(token) || ValidateJwtToken(token) == null) context.Result = new UnauthorizedResult();
    }

    private string? ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true, // Configúralo si necesitas validar el emisor
                ValidateAudience = true, // Configúralo si necesitas validar la audiencia
                ValidIssuer = "AdMeetI",
                ValidAudience = "AdMeetU",
                ValidateLifetime = true
            }, out var validatedToken);
            Console.WriteLine(validatedToken);
            return token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null!;
        }
    }
}