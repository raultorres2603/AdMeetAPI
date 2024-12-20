using AdMeet.Contexts;
using AdMeet.Inter;
using AdMeet.Models;
using Microsoft.EntityFrameworkCore;

namespace AdMeet.Services;

public class CategoryService(AppDbContext context, ILogger<ICategoryService> logger) : ICategoryService
{
    // Get categories from DB
    public async Task<List<Category>> GetCategories()
    {
        try
        {
            logger.LogInformation("Getting categories");
            return await context.Category.ToListAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

    // Create a new category
    public async Task<string> CreateCategory(Category c)
    {
        try
        {
            logger.LogInformation("Creating category");
            await context.Category.AddAsync(c);
            await context.SaveChangesAsync();
            logger.LogInformation("Done creating category");
            return "OK";
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }
}