using AdMeet.Contexts;
using AdMeet.Models;
using Microsoft.EntityFrameworkCore;

namespace AdMeet.Services;

public class UserServices(AppDbContext context)
{
    public async Task<string?> LogIn(User us)
    {
        // We check if user exists
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == us.Email);
        if (user == null)
            return null;
        if (!user.Password!.Equals(us.Password))
            return null;
        return new Jwt().GenerateToken(user!);
    }

    public async Task<List<User>> GetUsers()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<string> Register(User u)
    {
        var uExist = await context.Users.FirstOrDefaultAsync(cU => cU.Email!.Equals(u.Email));
        if (uExist != null) return "UAE";
        await context.Users.AddAsync(u);
        await context.SaveChangesAsync();
        return "OK";
    }
}