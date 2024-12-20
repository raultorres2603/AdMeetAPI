using AdMeet.Contexts;
using AdMeet.Inter;
using AdMeet.Models;
using Microsoft.EntityFrameworkCore;

namespace AdMeet.Services;

public class UserServices(AppDbContext context, IJwt jwt, ILogger<IUserServices> logger) : IUserServices
{
    public async Task<string?> LogIn(User us)
    {
        // We check if user exists
        logger.LogInformation($"Logging in {us.Email}");
        var user = await context.Users.Include(user => user.Profile).FirstOrDefaultAsync(u => u.Email == us.Email);
        if (user == null)
        {
            logger.LogError($"User not found {us.Email}");
            return null;
        }

        if (BCrypt.Net.BCrypt.EnhancedVerify(us.Password, user.Password))
        {
            logger.LogInformation($"Password correct {us.Password} {user.Password}");
            return jwt.GenerateToken(user);
        }

        logger.LogError($"Password incorrect {us.Password} {user.Password}");
        return null;
    }

    public async Task<List<User>> GetUsers()
    {
        logger.LogInformation("Getting users");
        return await context.Users.ToListAsync();
    }

    public async Task<string> Register(User u)
    {
        logger.LogInformation($"Registering {u.Email}");
        var uExist = await context.Users.FirstOrDefaultAsync(cU => cU.Email!.Equals(u.Email));
        if (uExist != null)
        {
            logger.LogError($"User {u.Email} already exists");
            return "UAE";
        }

        u.Password = u.HashPassword();
        logger.LogInformation($"Done registering {u.Email}");
        await context.Users.AddAsync(u);
        logger.LogInformation("Saving changes");
        await context.SaveChangesAsync();
        return "OK";
    }

    public async Task<(User, string)> GetInfo(string vJwt)
    {
        try
        {
            logger.LogInformation($"Getting info {vJwt}");
            var user = jwt.DecodeToken(vJwt);
            logger.LogInformation($"Done getting info {vJwt}");
            user = await GetUser(user.Email!);
            logger.LogInformation($"Done getting info {vJwt}");
            var newTok = jwt.GenerateToken(user);
            if (newTok == "")
            {
                logger.LogError("Token not validated");
                throw new Exception("Token not validated");
            }

            Console.WriteLine(newTok, user);
            return (user, newTok);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new Exception(e.Message);
        }
    }

    public async Task<string> UpdateProf(User u)
    {
        try
        {
            logger.LogInformation($"Updating profile {u.Email}");
            var user = await context.Users.Include(user => user.Profile).FirstOrDefaultAsync(u => u.Email == u.Email);
            if (user == null)
            {
                logger.LogError("User not found");
                throw new Exception("User not found");
            }

            user.Profile.Name = u.Profile.Name;
            user.Profile.LastName = u.Profile.LastName;
            user.Profile.City = u.Profile.City;
            user.Profile.Country = u.Profile.Country;
            user.Profile.ZipCode = u.Profile.ZipCode;
            try
            {
                logger.LogInformation("Saving changes");
                await context.SaveChangesAsync();
                return "OK";
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new Exception(e.Message);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new Exception(e.Message);
        }
    }

    public async Task<User> GetUser(string email)
    {
        logger.LogInformation($"Getting user {email}");
        var user = await context.Users.Include(user => user.Profile).FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            logger.LogError("User not found");
            throw new Exception("User not found");
        }

        return user;
    }
}