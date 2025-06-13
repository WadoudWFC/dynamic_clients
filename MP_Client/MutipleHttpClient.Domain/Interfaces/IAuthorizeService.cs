using MutipleHttpClient.Domain.Security.Authorization;

namespace MutipleHttpClient.Domain;

public interface IAuthorizeService
{
    // TO DO: Make sure to implement this interface!
    Task<Result<ProfileType>> GetUserProfileTypeAsync(Guid userGuid);
    Task<Result<object>> ValidateDossierAccessAsync(Guid userGuid, Guid dossierGuid, AccessType requiredAccess);
    Task<bool> VerifyOwnerShipAync(Guid userGuid, string resourceType, Guid resourceId);
    Task<bool> HasPermissionsAsync(Guid userGuid, string permissionCode);
    Task<SecurityContext> GetSecurityContextAsync(Guid userGuid);
    Task InvalidateAuthorizationCacheAsync(Guid userGuid);
}