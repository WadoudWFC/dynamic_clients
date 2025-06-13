namespace MutipleHttpClient.Domain.Security.Authorization
{
    public record SecurityContext(ProfileType ProfileType, IReadOnlyCollection<string> Permissions, IReadOnlyDictionary<string, int> InternalIds);
}
