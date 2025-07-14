namespace MutipleHttpClient.Domain;

public class SecurityHeadersOptions
{
    public bool ForceHSTS { get; set; } = true;
    public int HSTSMaxAge { get; set; } = 31536000; // 1 year
    public bool IncludeSubDomains { get; set; } = true;
    public bool Preload { get; set; } = true;

    public string ContentSecurityPolicy { get; set; } =
        "default-src 'self'; " +
        "script-src 'self' 'unsafe-inline'; " +
        "style-src 'self' 'unsafe-inline'; " +
        "img-src 'self' data: https:; " +
        "connect-src 'self'; " +
        "frame-ancestors 'none';";
}
