namespace MultipleHttpClient.Application.Services.Security
{
    public class RequireAdminAttribute : RequireProfileAttribute
    {
        public RequireAdminAttribute() : base(1) { }
    }
}
