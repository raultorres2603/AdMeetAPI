using Microsoft.AspNetCore.Mvc.Filters;

namespace AdMeet.Attributes;

public class Tracker : ActionFilterAttribute
{
    private readonly ILogger<Tracker> _logger;

    public Tracker(ILogger<Tracker> logger)
    {
        _logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Aquí puedes registrar la información antes de que se ejecute el endpoint
        var endpoint = context.ActionDescriptor.DisplayName;

        _logger.LogInformation($"Endpoint: {endpoint}");
    }
}