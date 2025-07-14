using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class AccountLockoutService : IAccountLockoutService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<AccountLockoutService> _logger;

    // SECURITY: Progressive lockout configuration
    private static readonly Dictionary<int, TimeSpan> LockoutDurations = new()
        {
            { 3, TimeSpan.FromMinutes(5) },
            { 5, TimeSpan.FromMinutes(15) },
            { 7, TimeSpan.FromMinutes(30) },
            { 10, TimeSpan.FromHours(1) },
            { 15, TimeSpan.FromHours(24) }
        };

    private const int MAX_ATTEMPTS_BEFORE_LOCKOUT = 3;
    private const int ATTEMPT_WINDOW_MINUTES = 15;

    public AccountLockoutService(IMemoryCache cache, ILogger<AccountLockoutService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> IsAccountLockedAsync(string identifier)
    {
        var cacheKey = $"lockout_{identifier}";
        var lockoutInfo = _cache.Get<AccountLockoutInfo>(cacheKey);

        if (lockoutInfo == null) return false;

        if (lockoutInfo.LockedUntil > DateTime.UtcNow)
        {
            _logger.LogWarning("Account {0} is locked until {1}",
                identifier, lockoutInfo.LockedUntil);
            return true;
        }

        // Lockout expired, clean up
        _cache.Remove(cacheKey);
        return false;
    }

    public async Task RecordFailedAttemptAsync(string identifier)
    {
        var cacheKey = $"lockout_{identifier}";
        var lockoutInfo = _cache.Get<AccountLockoutInfo>(cacheKey) ?? new AccountLockoutInfo();

        // Clean old attempts outside the window
        var cutoffTime = DateTime.UtcNow.AddMinutes(-ATTEMPT_WINDOW_MINUTES);
        lockoutInfo.FailedAttempts.RemoveAll(attempt => attempt < cutoffTime);

        // Add new failed attempt
        lockoutInfo.FailedAttempts.Add(DateTime.UtcNow);
        lockoutInfo.TotalFailedAttempts++;

        // Calculate lockout duration based on total failures
        var lockoutDuration = GetLockoutDuration(lockoutInfo.TotalFailedAttempts);

        if (lockoutInfo.FailedAttempts.Count >= MAX_ATTEMPTS_BEFORE_LOCKOUT)
        {
            lockoutInfo.LockedUntil = DateTime.UtcNow.Add(lockoutDuration);
            lockoutInfo.FailedAttempts.Clear(); // Reset sliding window after lockout

            _logger.LogWarning("Account {0} locked until {1} after {2} total failed attempts",
                identifier, lockoutInfo.LockedUntil, lockoutInfo.TotalFailedAttempts);
        }

        // FIX: Cache with appropriate expiration AND SIZE
        var cacheExpiration = lockoutInfo.LockedUntil > DateTime.UtcNow
            ? lockoutInfo.LockedUntil.Add(TimeSpan.FromMinutes(5))
            : DateTime.UtcNow.AddMinutes(ATTEMPT_WINDOW_MINUTES + 5);

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = cacheExpiration,
            Size = 1, // CRITICAL: Specify size when SizeLimit is configured
            Priority = CacheItemPriority.Normal
        };

        _cache.Set(cacheKey, lockoutInfo, cacheOptions);
    }

    public async Task RecordSuccessfulLoginAsync(string identifier)
    {
        var cacheKey = $"lockout_{identifier}";
        var lockoutInfo = _cache.Get<AccountLockoutInfo>(cacheKey);

        if (lockoutInfo != null)
        {
            // Reset sliding window attempts but keep total for progressive lockout
            lockoutInfo.FailedAttempts.Clear();
            lockoutInfo.LockedUntil = DateTime.MinValue;

            // FIX: Include size in cache options
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddHours(24),
                Size = 1, // CRITICAL: Specify size
                Priority = CacheItemPriority.Normal
            };

            _cache.Set(cacheKey, lockoutInfo, cacheOptions);

            _logger.LogInformation("Successful login for {0}, reset sliding window", identifier);
        }
    }

    public async Task<TimeSpan?> GetLockoutTimeRemainingAsync(string identifier)
    {
        var lockoutInfo = _cache.Get<AccountLockoutInfo>($"lockout_{identifier}");

        if (lockoutInfo?.LockedUntil > DateTime.UtcNow)
        {
            return lockoutInfo.LockedUntil - DateTime.UtcNow;
        }

        return null;
    }

    public async Task<int> GetFailedAttemptsCountAsync(string identifier)
    {
        var lockoutInfo = _cache.Get<AccountLockoutInfo>($"lockout_{identifier}");
        return lockoutInfo?.FailedAttempts.Count ?? 0;
    }

    private static TimeSpan GetLockoutDuration(int totalFailedAttempts)
    {
        foreach (var threshold in LockoutDurations.OrderByDescending(x => x.Key))
        {
            if (totalFailedAttempts >= threshold.Key)
            {
                return threshold.Value;
            }
        }
        return TimeSpan.FromMinutes(1);
    }
}