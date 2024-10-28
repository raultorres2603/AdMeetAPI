using Microsoft.AspNetCore.Mvc.Filters;

namespace AdMeet.Attributes;

public class Tracker(ILogger<Tracker> logger) : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Aquí puedes registrar la información antes de que se ejecute el endpoint
        var endpoint = context.ActionDescriptor.DisplayName;

        logger.LogInformation($"Endpoint: {endpoint}");
    }
}