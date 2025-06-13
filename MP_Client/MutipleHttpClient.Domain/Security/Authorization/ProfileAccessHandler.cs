using Microsoft.AspNetCore.Authorization;

namespace MutipleHttpClient.Domain.Security.Authorization
{
    public class ProfileAccessHandler : AuthorizationHandler<ProfileAccessRequirement>
    {
        private readonly MutipleHttpClient.Domain.IAuthorizeService _authorizationService;
        public ProfileAccessHandler(IAuthorizeService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProfileAccessRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == SecurityConstants.ProfileTypeClaim))
            {
                var profileType = (ProfileType)Enum.Parse(typeof(ProfileType), context.User.FindFirst(SecurityConstants.ProfileTypeClaim)!.Value);
                if (requirement.AllowedProfiles.Contains(profileType))
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                // Fallback to service check
                var userGuid = Guid.Parse(context.User.FindFirst(SecurityConstants.UserGuidClaim)!.Value);
                var result = await _authorizationService.GetUserProfileTypeAsync(userGuid);
                if (result.IsSuccess && requirement.AllowedProfiles.Contains(result.Value))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
