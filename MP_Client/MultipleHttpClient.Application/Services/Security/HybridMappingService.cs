using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class HybridMappingService : IIdMappingService
{
    private readonly IIdMappingService _legacyService;
    private readonly IIdMappingService _deterministicService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<HybridMappingService> _logger;

    public HybridMappingService(IdMappingService legacyService, DeterministicIdMappingService deterministicService, IConfiguration configuration, ILogger<HybridMappingService> logger)
    {
        _legacyService = legacyService;
        _deterministicService = deterministicService;
        _configuration = configuration;
        _logger = logger;
    }

    public Guid GetGuidForUserId(int userId)
    {
        var useDeterministic = _configuration.GetValue<bool>("MappingService:UseDeterministic", true);

        if (useDeterministic)
        {
            try
            {
                var guid = _deterministicService.GetGuidForUserId(userId);
                _logger.LogDebug("Used deterministic service for UserId {0}", userId);
                return guid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Deterministic service failed for UserId {0}, falling back to legacy", userId);
                return _legacyService.GetGuidForUserId(userId);
            }
        }

        _logger.LogDebug("Used legacy service for UserId {0}", userId);
        return _legacyService.GetGuidForUserId(userId);
    }

    public int? GetUserIdForGuid(Guid guid)
    {
        var useDeterministic = _configuration.GetValue<bool>("MappingService:UseDeterministic", true);

        if (useDeterministic)
        {
            try
            {
                var userId = _deterministicService.GetUserIdForGuid(guid);
                if (userId.HasValue)
                {
                    return userId;
                }

                // Fallback to legacy if deterministic doesn't have the mapping
                _logger.LogDebug("Deterministic service couldn't find GUID {0}, trying legacy", guid);
                return _legacyService.GetUserIdForGuid(guid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Deterministic service failed for GUID {0}, falling back to legacy", guid);
                return _legacyService.GetUserIdForGuid(guid);
            }
        }

        return _legacyService.GetUserIdForGuid(guid);
    }

    public void RemoveMapping(Guid guid)
    {
        var useDeterministic = _configuration.GetValue<bool>("MappingService:UseDeterministic", true);

        if (useDeterministic)
        {
            try
            {
                _deterministicService.RemoveMapping(guid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove mapping from deterministic service for GUID {0}", guid);
            }
        }

        // Always try to remove from legacy as well during transition
        try
        {
            _legacyService.RemoveMapping(guid);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove mapping from legacy service for GUID {0}", guid);
        }
    }
}
