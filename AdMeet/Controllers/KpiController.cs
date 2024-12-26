using AdMeet.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AdMeet.Controllers;

[ApiController]
[AdminAuth]
[Produces("application/json")]
[Route("api/admin")]
public class KpiController : ControllerBase
{
    // Get all KPI data from DB
    [HttpGet("kpi", Name = "GetKpi")]
    public IActionResult GetKpi()
    {
        return Ok();
    }
}