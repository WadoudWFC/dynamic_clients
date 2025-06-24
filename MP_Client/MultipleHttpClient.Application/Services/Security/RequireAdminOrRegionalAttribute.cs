namespace MultipleHttpClient.Application.Services.Security
{
    public class RequireAdminOrRegionalAttribute : RequireProfileAttribute
    {
        public RequireAdminOrRegionalAttribute() : base(1, 2) { }
    }
}
