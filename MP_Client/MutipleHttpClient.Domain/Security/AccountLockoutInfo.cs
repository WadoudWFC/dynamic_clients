namespace MutipleHttpClient.Domain;

public class AccountLockoutInfo
{
    public List<DateTime> FailedAttempts { get; set; } = new();
    public int TotalFailedAttempts { get; set; }
    public DateTime LockedUntil { get; set; } = DateTime.MinValue;
}
