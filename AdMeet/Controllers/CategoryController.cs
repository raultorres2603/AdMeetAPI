using AdMeet.Attributes;
using AdMeet.Inter;
using Microsoft.AspNetCore.Mvc;

namespace AdMeet.Controllers;

[ApiController]
[Route("/api/category")]
[TypeFilter(typeof(Tracker))]
public class CategoryController(ICategoryService categoryServices, ILogger<ICategoryService> logger) : ControllerBase
{
    // Get all categories

    [HttpGet("all", Name = "GetCategories")]
    public async Task<IActionResult> GetCategories()
    {
        logger.LogInformation("Getting categories");
        try
        {
            var categories = await categoryServices.GetCategories();
            logger.LogInformation("Done getting categories");
            return Ok(categories);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}