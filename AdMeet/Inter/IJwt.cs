using AdMeet.Models;

namespace AdMeet.Inter;

public interface IJwt
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiresInMinutes { get; set; }
    public string GenerateToken(User u);
    User DecodeToken(string vJwt);
}