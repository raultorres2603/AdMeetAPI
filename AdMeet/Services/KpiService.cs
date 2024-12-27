using AdMeet.Contexts;
using AdMeet.Inter;
using AdMeet.Models;

namespace AdMeet.Services;

public class KpiService(AppDbContext dbContext, ILogger<KpiService> logger) : IKpiService
{
    public Task<object> GetAllKpi()
    {
        throw new NotImplementedException();
    }

    public bool InsertKpi(Kpi kpi)
    {
        try
        {
            logger.LogInformation("Adding {KPI} to DB", kpi.ToString());
            dbContext.Kpi.Add(kpi);
            logger.LogInformation("Added {KPI} to DB", kpi.ToString());
            logger.LogInformation("Saving KPI...");
            dbContext.SaveChanges();
            logger.LogInformation("Saved KPI to DB");
            return true;
        }
        catch (Exception e)
        {
            logger.LogError("Couldn't save {KPI} KPI on DB. Error message: {eMessage}", kpi.ToString(), e.Message);
            return false;
        }
    }
}