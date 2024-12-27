using AdMeet.Models;

namespace AdMeet.Inter;

public interface IKpiService
{
    public Task<object> GetAllKpi();
    public bool InsertKpi(Kpi kpi);
}