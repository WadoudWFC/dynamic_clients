using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class MappingHealthCheck : IHealthCheck
{
    private readonly IIdMappingService _idMappingService;
    private readonly IReferenceDataMappingService _referenceDataMappingService;
    private readonly ILogger<MappingHealthCheck> _logger;

    public MappingHealthCheck(IIdMappingService idMappingService, IReferenceDataMappingService referenceDataMappingService, ILogger<MappingHealthCheck> logger)
    {
        _idMappingService = idMappingService;
        _referenceDataMappingService = referenceDataMappingService;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Test user ID mapping consistency
            var testUserId = 999999; // Use a test ID that won't conflict
            var guid1 = _idMappingService.GetGuidForUserId(testUserId);
            var guid2 = _idMappingService.GetGuidForUserId(testUserId);

            if (guid1 != guid2)
            {
                _logger.LogError("User mapping inconsistency detected: {0} != {1}", guid1, guid2);
                return HealthCheckResult.Unhealthy("User ID mapping inconsistency detected");
            }

            // Test reference data mapping consistency
            var testRefId = 999999;
            var refGuid1 = _referenceDataMappingService.GetOrCreateGuidForReferenceId(testRefId, "HealthCheck");
            var refGuid2 = _referenceDataMappingService.GetOrCreateGuidForReferenceId(testRefId, "HealthCheck");

            if (refGuid1 != refGuid2)
            {
                _logger.LogError("Reference mapping inconsistency detected: {0} != {1}", refGuid1, refGuid2);
                return HealthCheckResult.Unhealthy("Reference data mapping inconsistency detected");
            }

            // Test reverse lookup (if supported)
            var retrievedUserId = _idMappingService.GetUserIdForGuid(guid1);
            if (retrievedUserId != testUserId)
            {
                _logger.LogWarning("Reverse lookup failed for user mapping: expected {0}, got {1}",
                    testUserId, retrievedUserId);
                // This is a warning, not a failure, as reverse lookup might not always work
            }

            // Clean up test data
            _idMappingService.RemoveMapping(guid1);
            _referenceDataMappingService.RemoveReferenceMapping(refGuid1, "HealthCheck");

            return HealthCheckResult.Healthy("Mapping services are operating correctly");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Mapping health check failed");
            return HealthCheckResult.Unhealthy($"Mapping services failed: {ex.Message}");
        }
    }
}
