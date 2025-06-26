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
                // Return 403 Forbidden with proper error object
                context.Result = new ObjectResult(new
                {
                    error = "User account is inactive",
                    code = "INACTIVE_ACCOUNT"
                })
                {
                    StatusCode = 403
                };
                return;
            }

            // Get internal profile ID from hidden claim
            var profileIdClaim = user.FindFirst("internal_profile_id")?.Value;
            if (string.IsNullOrEmpty(profileIdClaim) || !int.TryParse(profileIdClaim, out var profileId))
            {
                context.Result = new ObjectResult(new
                {
                    error = "Invalid profile information",
                    code = "INVALID_PROFILE"
                })
                {
                    StatusCode = 403
                };
                return;
            }

            if (!_allowedProfiles.Contains(profileId))
            {
                context.Result = new ObjectResult(new
                {
                    error = "Access denied: Insufficient permissions",
                    code = "INSUFFICIENT_PERMISSIONS",
                    requiredProfiles = _allowedProfiles,
                    userProfile = profileId
                })
                {
                    StatusCode = 403
                };
                return;
            }
        }
    }
}
