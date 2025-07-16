using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MultipleHttpClient.Application;

public class ApiRouteSegregationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiRouteSegregationMiddleware> _logger;

    private static readonly Dictionary<string, int[]> RoutePermissions = new()
        {
            { "/api/v1/admin", new[] { 1, 2 } },
            { "/api/admin", new[] { 1, 2 } },

            { "/api/v1/superadmin", new[] { 1 } },

            { "/api/bff/v2", new[] { 3 } },
            { "/api/user", new[] { 3 } }
        };

    public ApiRouteSegregationMiddleware(RequestDelegate next, ILogger<ApiRouteSegregationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant();

        if (string.IsNullOrEmpty(path))
        {
            await _next(context);
            return;
        }

        if (IsPublicEndpoint(path))
        {
            await _next(context);
            return;
        }

        var restrictedRoute = RoutePermissions.FirstOrDefault(r => path.StartsWith(r.Key.ToLowerInvariant()));

        if (restrictedRoute.Key != null)
        {
            var profileIdClaim = context.User?.FindFirst("internal_profile_id")?.Value;

            if (!int.TryParse(profileIdClaim, out int userProfileId))
            {
                await BlockAccess(context, "Invalid or missing profile information");
                return;
            }

            if (!restrictedRoute.Value.Contains(userProfileId))
            {
                var allowedProfiles = string.Join(", ", restrictedRoute.Value);
                _logger.LogWarning("SECURITY: Unauthorized access attempt to {0} by user with profile {1}. Allowed: {2}",
                    path, userProfileId, allowedProfiles);

                await BlockAccess(context, $"Access denied: Route requires profile {allowedProfiles}, but user has profile {userProfileId}");
                return;
            }

            _logger.LogInformation("Access granted to {0} for user with profile {1}", path, userProfileId);
        }

        await _next(context);
    }

    private static bool IsPublicEndpoint(string path)
    {
        var publicPaths = new[]
        {
                "/api/user/login",
                "/api/user/cantrylogin",
                "/api/user/forgetpassword",
                "/api/user/register",
                "/swagger",
                "/health"
            };

        return publicPaths.Any(publicPath => path.StartsWith(publicPath.ToLowerInvariant()));
    }

    private async Task BlockAccess(HttpContext context, string reason)
    {
        context.Response.StatusCode = 403;
        context.Response.ContentType = "application/json";

        var errorResponse = new
        {
            error = "API_ROUTE_FORBIDDEN",
            message = reason,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path.Value
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}

