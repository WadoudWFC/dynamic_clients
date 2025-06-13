using Microsoft.AspNetCore.Authorization;

namespace MutipleHttpClient.Domain.Security.Authorization
{
    public record ProfileAccessRequirement(params ProfileType[] AllowedProfiles) : IAuthorizationRequirement;
}
