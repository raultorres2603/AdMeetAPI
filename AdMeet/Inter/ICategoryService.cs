using AdMeet.Models;

namespace AdMeet.Inter;

public interface ICategoryService
{
    public Task<List<Category>> GetCategories();
    public Task<string> CreateCategory(Category c);
}