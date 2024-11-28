using AdMeet.Contexts;
using AdMeet.Inter;
using AdMeet.Models;
using Microsoft.EntityFrameworkCore;

namespace AdMeet.Services;

public class UserServices(AppDbContext context, IJwt jwt) : IUserServices
{
    public async Task<string?> LogIn(User us)
    {
        // We check if user exists
        var user = await context.Users.Include(user => user.Profile).FirstOrDefaultAsync(u => u.Email == us.Email);
        if (user == null)
            return null;
        if (BCrypt.Net.BCrypt.EnhancedVerify(us.Password, user.Password)) return jwt.GenerateToken(user);
        Console.WriteLine($"Password incorrect {us.Password} {user.Password}");
        return null;
    }

    public async Task<List<User>> GetUsers()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<string> Register(User u)
    {
        var uExist = await context.Users.FirstOrDefaultAsync(cU => cU.Email!.Equals(u.Email));
        if (uExist != null) return "UAE";
        u.Password = u.HashPassword();
        await context.Users.AddAsync(u);
        await context.SaveChangesAsync();
        return "OK";
    }

    public async Task<(User, string)> GetInfo(string vJwt)
    {
        try
        {
            var user = jwt.DecodeToken(vJwt);
            user = await GetUser(user.Email);
            var newTok = jwt.GenerateToken(user);
            if (newTok == "") throw new Exception("Token not validated");
            Console.WriteLine(newTok, user);
            return (user, newTok);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<User> GetUser(string email)
    {
        var user = await context.Users.Include(user => user.Profile).FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) throw new Exception("User not found");
        return user;
    }
}