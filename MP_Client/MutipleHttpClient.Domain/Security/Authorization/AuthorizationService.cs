namespace MutipleHttpClient.Domain.Security.Authorization
{
    public class AuthorizationService : IAuthorizeService
    {
        private readonly IIdMappingService _idMappingService;

        public Task<SecurityContext> GetSecurityContextAsync(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ProfileType>> GetUserProfileTypeAsync(Guid userGuid)
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
}
