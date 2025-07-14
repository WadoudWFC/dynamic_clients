using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityHeadersMiddleware> _logger;
    private readonly SecurityHeadersOptions _options;

    public SecurityHeadersMiddleware(RequestDelegate next, ILogger<SecurityHeadersMiddleware> logger, IOptions<SecurityHeadersOptions> options)
    {
        _next = next;
        _logger = logger;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        AddSecurityHeaders(context);

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in request pipeline");
            EnsureSecurityHeaders(context);
        }

        // Optional: Add headers based on response status
        AddResponseSpecificHeaders(context);
    }

    private void AddSecurityHeaders(HttpContext context)
    {
        var response = context.Response;
        var request = context.Request;
        if (request.IsHttps || _options.ForceHSTS)
        {
            var hstsValue = $"max-age={_options.HSTSMaxAge}";
            if (_options.IncludeSubDomains) hstsValue += "; includeSubDomains";
            if (_options.Preload) hstsValue += "; preload";

            response.Headers["Strict-Transport-Security"] = hstsValue;
        }

        if (!string.IsNullOrEmpty(_options.ContentSecurityPolicy))
        {
            response.Headers["Content-Security-Policy"] = _options.ContentSecurityPolicy;
        }
        response.Headers["X-Frame-Options"] = "DENY";

        response.Headers["X-Content-Type-Options"] = "nosniff";

        response.Headers["X-XSS-Protection"] = "1; mode=block";

        response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        response.Headers["Permissions-Policy"] = "camera=(), microphone=(), location=(), geolocation=()";

        response.Headers["Cross-Origin-Embedder-Policy"] = "require-corp";
        response.Headers["Cross-Origin-Opener-Policy"] = "same-origin";
        response.Headers["Cross-Origin-Resource-Policy"] = "same-origin";

        response.Headers.Remove("Server");
        response.Headers.Remove("X-Powered-By");
        response.Headers.Remove("X-AspNet-Version");
        response.Headers.Remove("X-AspNetMvc-Version");

        _logger.LogDebug("Security headers added for {Method} {Path}",
            request.Method, request.Path);
    }

    private void EnsureSecurityHeaders(HttpContext context)
    {
        var criticalHeaders = new[] { "X-Content-Type-Options", "X-Frame-Options" };

        foreach (var header in criticalHeaders)
        {
            if (!context.Response.Headers.ContainsKey(header))
            {
                _logger.LogWarning("Critical security header {Header} was missing", header);
                AddSecurityHeaders(context);
                break;
            }
        }
    }

    private void AddResponseSpecificHeaders(HttpContext context)
    {
        var response = context.Response;

        if (IsApiResponse(context))
        {
            response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            response.Headers["Pragma"] = "no-cache";
        }

        if (IsFileDownload(context))
        {
            response.Headers["X-Download-Options"] = "noopen";
            response.Headers["X-Content-Type-Options"] = "nosniff";
        }
    }

    private static bool IsApiResponse(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/api") ||
               context.Response.ContentType?.Contains("application/json") == true;
    }

    private static bool IsFileDownload(HttpContext context)
    {
        return context.Response.Headers.ContainsKey("Content-Disposition");
    }
}
