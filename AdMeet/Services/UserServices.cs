using AdMeet.Contexts;
using AdMeet.Models;
using Microsoft.EntityFrameworkCore;

namespace AdMeet.Services;

public class UserServices
{
    private readonly AppDbContext _context;

    public UserServices(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string?> LogIn(User us)
    {
        // We check if user exists
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == us.Email);
        if (user == null)
            return null;
        if (!user.Password!.Equals(us.Password))
            return null;
        return new Jwt().GenerateToken(user!);
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }
}