using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MultipleHttpClient.Application;

public class XssProtectionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<XssProtectionMiddleware> _logger;

    public XssProtectionMiddleware(RequestDelegate next, ILogger<XssProtectionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"] = "DENY";
        context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
        context.Response.Headers["Content-Security-Policy"] =
            "default-src 'self'; script-src 'self'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:; object-src 'none';";

        if (context.Request.HasFormContentType ||
            context.Request.ContentType?.Contains("application/json") == true)
        {
            await ValidateRequestContent(context);
        }

        await _next(context);
    }
    private async Task ValidateRequestContent(HttpContext context)
    {
        if (context.Request.Body.CanRead)
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (ContainsPotentialXss(body))
            {
                _logger.LogWarning("Potential XSS attempt detected from {IP} on {Path}",
                    context.Connection.RemoteIpAddress, context.Request.Path);

                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    error = "Invalid request content detected",
                    timestamp = DateTime.UtcNow
                }));
                return;
            }
        }
    }
    private bool ContainsPotentialXss(string content)
    {
        if (string.IsNullOrEmpty(content)) return false;

        var dangerous = new[]
        {
            "<script", "javascript:", "vbscript:", "onload=", "onerror=",
            "onclick=", "onmouseover=", "eval(", "expression(", "<iframe"
        };

        return dangerous.Any(pattern =>
            content.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }
}
