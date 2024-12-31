using AdMeet.Models;

namespace AdMeet.Inter;

public interface IKpiService
{
    public IKpiUsageData GetAllKpi();
    public bool InsertKpi(Kpi kpi);

    public List<User> GetUsersFromDiffCountry(string country);
}