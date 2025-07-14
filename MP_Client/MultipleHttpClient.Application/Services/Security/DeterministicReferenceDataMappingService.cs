using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class DeterministicReferenceDataMappingService : IReferenceDataMappingService
{
    private readonly string _applicationSalt;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<DeterministicReferenceDataMappingService> _logger;
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, int>> _reverseMap = new();
    private readonly bool _enableCache;
    private readonly TimeSpan _referenceCacheExpiration;

    public DeterministicReferenceDataMappingService(IConfiguration configuration, IMemoryCache memoryCache, ILogger<DeterministicReferenceDataMappingService> logger)
    {
        _applicationSalt = configuration.GetValue<string>("MappingService:ApplicationSalt") ?? throw new InvalidOperationException("MappingService:ApplicationSalt configuration is required");
        _memoryCache = memoryCache;
        _logger = logger;
        _enableCache = configuration.GetValue<bool>("MappingService:EnableCache", true);
        _referenceCacheExpiration = TimeSpan.FromDays(configuration.GetValue<int>("MappingService:ReferenceCacheExpirationDays", 1));
        _logger.LogInformation("DeterministicReferenceDataMappingService initialized");
    }

    public Guid GetOrCreateGuidForReferenceId(int referenceId, string entityType)
    {
        var cacheKey = $"ref_guid_{entityType}_{referenceId}";

        if (_enableCache && _memoryCache.TryGetValue(cacheKey, out Guid cachedGuid))
        {
            return cachedGuid;
        }

        var guid = GenerateDeterministicGuid(referenceId, entityType);

        // Store both forward and reverse mappings
        if (_enableCache)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = _referenceCacheExpiration,
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30),
                Priority = CacheItemPriority.High,
                Size = 1
            };

            _memoryCache.Set(cacheKey, guid, cacheOptions);
            _memoryCache.Set($"guid_ref_{entityType}_{guid}", referenceId, cacheOptions);
        }

        // Store in reverse map
        var entityReverseMap = _reverseMap.GetOrAdd(entityType, _ => new ConcurrentDictionary<Guid, int>());
        entityReverseMap.TryAdd(guid, referenceId);

        _logger.LogDebug("Generated GUID {0} for {1} ID {2}", guid, entityType, referenceId);
        return guid;
    }

    public int? GetReferenceIdForGuid(Guid guid, string entityType)
    {
        var cacheKey = $"guid_ref_{entityType}_{guid}";

        // Check cache first
        if (_enableCache && _memoryCache.TryGetValue(cacheKey, out int cachedReferenceId))
        {
            return cachedReferenceId;
        }

        // Check in-memory reverse map
        if (_reverseMap.TryGetValue(entityType, out var entityMap) &&
            entityMap.TryGetValue(guid, out int mappedReferenceId))
        {
            if (_enableCache)
            {
                _memoryCache.Set(cacheKey, mappedReferenceId, _referenceCacheExpiration);
            }
            return mappedReferenceId;
        }

        _logger.LogWarning("Could not find {0} ID for GUID {1} - reverse lookup failed", entityType, guid);
        return null;
    }

    public void RemoveReferenceMapping(Guid guid, string entityType)
    {
        if (_reverseMap.TryGetValue(entityType, out var entityMap) &&
            entityMap.TryRemove(guid, out int referenceId))
        {
            if (_enableCache)
            {
                _memoryCache.Remove($"ref_guid_{entityType}_{referenceId}");
                _memoryCache.Remove($"guid_ref_{entityType}_{guid}");
            }
            _logger.LogDebug("Removed mapping for {0} GUID {1} and ID {2}", entityType, guid, referenceId);
        }
    }

    private Guid GenerateDeterministicGuid(int id, string entityType)
    {
        // Create consistent input string
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
