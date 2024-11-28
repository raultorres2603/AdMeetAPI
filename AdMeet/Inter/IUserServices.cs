using AdMeet.Models;

namespace AdMeet.Inter;

public interface IUserServices
{
    Task<string?> LogIn(User us);
    Task<string> Register(User u);
    Task<List<User>> GetUsers();
    Task<(User, string)> GetInfo(string vJwt);
}