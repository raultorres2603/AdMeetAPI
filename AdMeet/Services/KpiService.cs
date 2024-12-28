using AdMeet.Contexts;
using AdMeet.Inter;
using AdMeet.Models;

namespace AdMeet.Services;

public class KpiService(AppDbContext dbContext, ILogger<KpiService> logger) : IKpiService
{
    public IKpiUsageData GetAllKpi()
    {
        // Get users logedIn from DB grouped by date
        logger.LogInformation("Picking users logedIn from DB grouped by date");
        var usersLogedIn = GetUsersLogedIn();

        // Get users registered from DB grouped by date
        logger.LogInformation("Picking users registered from DB grouped by date");
        var usersRegistered = GetUsersRegistered();
        // Return all KPI's data
        logger.LogInformation("Returning all KPI's data");
        return new KpiUsageData(usersLogedIn, usersRegistered);
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

    private List<UsersLogedIn> GetUsersLogedIn()
    {
        var usersLogedIn = dbContext.Kpi
            .Where(kpi => kpi.EndPoint == "/api/user/login") // Filtro por campo
            .GroupBy(kpi => DateOnly.FromDateTime(kpi.EnteredOn)) // Agrupamiento
            .Select(kpiGroup => new UsersLogedIn
            {
                Date = kpiGroup.Key,
                TotalUsersLogedIn = kpiGroup.Count()
            })
            .ToList();
        return usersLogedIn;
    }

    private List<UsersRegistered> GetUsersRegistered()
    {
        var usersRegistered = dbContext.Kpi
            .Where(kpi => kpi.EndPoint == "/api/user/register") // Filtro por campo
            .GroupBy(kpi => DateOnly.FromDateTime(kpi.EnteredOn)) // Agrupamiento
            .Select(kpiGroup => new UsersRegistered
            {
                Date = kpiGroup.Key,
                TotalUsersRegistered = kpiGroup.Count()
            })
            .ToList();
        return usersRegistered;
    }
}