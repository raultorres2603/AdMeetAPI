namespace AdMeet.Inter;

public interface IKpiUsageData
{
    public object UsersLogedIn { get; set; }
    public object UsersRegistered { get; set; }

    public object UsersFromDiffCountry { get; set; }
}