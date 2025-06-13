using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Interfaces.Dossier;
using MultipleHttpClient.Application.Interfaces.Security;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Security.Authorization;

namespace MultipleHttpClient.Application.Services.Security;

public class AuthorizationService : Interfaces.Security.IAuthorizeService
{
    private readonly IIdMappingService _idMappingService;
    private readonly IHttpDossierAglouService _dossierService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<AuthorizationService> _logger;
    public AuthorizationService(IIdMappingService idMappingService, IHttpDossierAglouService dossierService, IMemoryCache cache, ILogger<AuthorizationService> logger)
    {
        _idMappingService = idMappingService;
        _dossierService = dossierService;
        _cache = cache;
        _logger = logger;
    }

    public Task<SecurityContext> GetSecurityContextAsync(Guid userGuid)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<ProfileType>> GetUserProfileTypeAsync(Guid userGuid)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasPermissionsAsync(Guid userGuid, string permissionCode)
    {
        throw new NotImplementedException();
    }

    public Task InvalidateAuthorizationCacheAsync(Guid userGuid)
    {
        throw new NotImplementedException();
    }

    public Task<Result<object>> ValidateDossierAccessAsync(Guid userGuid, Guid dossierGuid, AccessType requiredAccess)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyOwnerShipAync(Guid userGuid, string resourceType, Guid resourceId)
    {
        throw new NotImplementedException();
    }
}
