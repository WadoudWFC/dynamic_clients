using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace MultipleHttpClient.Application.Services.Security
{
    public class RequireProfileAttribute : Attribute, IAuthorizationFilter
    {
        private readonly int[] _allowedProfiles;

        public RequireProfileAttribute(params int[] allowedProfiles)
        {
            _allowedProfiles = allowedProfiles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity?.IsAuthenticated ?? false)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Check if user is active
            var isActiveClaim = user.FindFirst("is_active")?.Value;
            if (isActiveClaim != "True")
            {
                context.Result = new ForbidResult("User account is inactive");
                return;
            }

            // Get internal profile ID from hidden claim
            var profileIdClaim = user.FindFirst("internal_profile_id")?.Value;
            if (string.IsNullOrEmpty(profileIdClaim) || !int.TryParse(profileIdClaim, out var profileId))
            {
                context.Result = new ForbidResult("Invalid profile information");
                return;
            }

            if (!_allowedProfiles.Contains(profileId))
            {
                context.Result = new ForbidResult($"Access denied: Required profile level not met. User profile: {profileId}");
            }
        }
    }
}
