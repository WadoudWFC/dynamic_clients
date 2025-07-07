using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class DeterministicIdMappingService : IIdMappingService
{
    private readonly string _applicationSalt;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<DeterministicIdMappingService> _logger;
    private readonly ConcurrentDictionary<Guid, int> _reverseMap = new ConcurrentDictionary<Guid, int>();
    private readonly bool _enableCache;
    public DeterministicIdMappingService(IConfiguration configuration, IMemoryCache memoryCache, ILogger<DeterministicIdMappingService> logger)
    {
        _applicationSalt = configuration.GetValue<string>("MappingService:ApplicationSalt") ?? throw new InvalidOperationException("MappingService:ApplicationSalt configuration is required");
        _memoryCache = memoryCache;
        _logger = logger;
        _enableCache = configuration.GetValue<bool>("MappingService:EnableCache", true);
        _logger.LogInformation("DeterministicIdMappingService initialized with salt hash: {0}", Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(_applicationSalt)))[..8]);
    }
    public Guid GetGuidForUserId(int userId)
    {
        var cacheKey = $"user_guid_{userId}";

        // Check cache first for performance
        if (_enableCache && _memoryCache.TryGetValue(cacheKey, out Guid cachedGuid))
        {
            return cachedGuid;
        }

        // Generate deterministic GUID
        var guid = GenerateDeterministicGuid(userId, "User");

        if (_enableCache)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(24),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7),
                Priority = CacheItemPriority.Normal,
                Size = 1
            };

            _memoryCache.Set(cacheKey, guid, cacheOptions);
            _memoryCache.Set($"guid_user_{guid}", userId, cacheOptions);
        }

        // Store in reverse map for quick lookup
        _reverseMap.TryAdd(guid, userId);

        _logger.LogDebug("Generated GUID {0} for UserId {1}", guid, userId);
        return guid;
    }

    public int? GetUserIdForGuid(Guid guid)
    {
        var cacheKey = $"guid_user_{guid}";

        if (_enableCache && _memoryCache.TryGetValue(cacheKey, out int cachedUserId))
        {
            return cachedUserId;
        }

        if (_reverseMap.TryGetValue(guid, out int mappedUserId))
        {
            if (_enableCache)
            {
                _memoryCache.Set(cacheKey, mappedUserId, TimeSpan.FromHours(24));
            }
            return mappedUserId;
        }

        _logger.LogWarning("Could not find UserId for GUID {0} - reverse lookup failed", guid);
        return null;
    }

    public void RemoveMapping(Guid guid)
    {
        if (_reverseMap.TryRemove(guid, out int userId))
        {
            if (_enableCache)
            {
                _memoryCache.Remove($"user_guid_{userId}");
                _memoryCache.Remove($"guid_user_{guid}");
            }
            _logger.LogDebug("Removed mapping for GUID {0} and UserId {1}", guid, userId);
        }
    }
    private Guid GenerateDeterministicGuid(int id, string entityType)
    {
        var input = $"{_applicationSalt}:{entityType}:{id}";

        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

        var guidBytes = new byte[16];
        Array.Copy(hashBytes, guidBytes, 16);

        guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | 0x50);

        guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80);

        return new Guid(guidBytes);
    }
}
