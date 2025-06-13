namespace MultipleHttpClient.Application.Interfaces.Security
{
    public static class RoleSecurity
    {
        public const int RegularUserProfileId = 3;
        public static Guid GetRoleGuid(int profileId)
        {
            // Deterministic GUID generation based on profileId
            return profileId == RegularUserProfileId ? new Guid("33333333-3333-3333-3333-333333333333") : new Guid(profileId.ToString("D32"));
        }
        public static bool IsRegularUser(int profileId) => profileId == RegularUserProfileId;
    }
}
