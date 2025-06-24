namespace MultipleHttpClient.Application.Services.Security
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";
        public string Secret { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int ExpirayMinutes { get; set; } = 60;
        public int RefreshTokenExpiryDays { get; set; } = 7; 
    }
}
