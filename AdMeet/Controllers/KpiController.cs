using AdMeet.Attributes;
using AdMeet.Inter;
using Microsoft.AspNetCore.Mvc;

namespace AdMeet.Controllers;

[ApiController]
[AdminAuth]
[Produces("application/json")]
[Route("api/admin")]
public class KpiController(IKpiService kpiService, ILogger<KpiController> logger) : ControllerBase
{
    // Get all KPI data from DB
    [HttpGet("kpi", Name = "GetKpi")]
    public IActionResult GetKpi()
    {
        logger.LogInformation("Access on {controller} {action}", nameof(KpiController), nameof(GetKpi));
        return Ok(kpiService.GetAllKpi());
    }
}