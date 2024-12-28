using AdMeet.Inter;

namespace AdMeet.Models;

public class KpiUsageData(object usersLogedIn, object usersRegistered) : IKpiUsageData
{
    public object UsersLogedIn { get; set; } = usersLogedIn;
    public object UsersRegistered { get; set; } = usersRegistered;
}