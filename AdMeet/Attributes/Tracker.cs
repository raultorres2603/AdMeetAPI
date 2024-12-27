using AdMeet.Inter;
using AdMeet.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdMeet.Attributes;

public class Tracker(ILogger<Tracker> logger, IKpiService kpiService) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Aquí puedes registrar la información antes de que se ejecute el endpoint
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Aquí puedes registrar la información luego de que se ejecute el endpoint
        var endpoint = context.HttpContext.Request.Path;
        logger.LogInformation("EndPoint: {Endpoint}", endpoint);
        var kpi = new Kpi(endpoint);
        logger.LogInformation("Kpi created: {EndPoint}, {Id}, {EnteredOn}", kpi.EndPoint, kpi.Id, kpi.EnteredOn);
        logger.LogInformation("Inserting on KPI's table on DB");
        kpiService.InsertKpi(kpi);
    }
}