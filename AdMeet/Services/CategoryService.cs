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
}