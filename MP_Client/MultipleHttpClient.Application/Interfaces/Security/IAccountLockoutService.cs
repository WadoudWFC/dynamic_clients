namespace MultipleHttpClient.Application;

public interface IAccountLockoutService
{
    Task<bool> IsAccountLockedAsync(string identifier);
    Task RecordFailedAttemptAsync(string identifier);
    Task RecordSuccessfulLoginAsync(string identifier);
    Task<TimeSpan?> GetLockoutTimeRemainingAsync(string identifier);
    Task<int> GetFailedAttemptsCountAsync(string identifier);
}
