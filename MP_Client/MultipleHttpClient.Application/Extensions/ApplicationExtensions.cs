using System.IO.Compression;
using System.Threading.RateLimiting;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MultipleHttpClient.Application.Commons.Behavior;
using MultipleHttpClient.Application.Dossier.Handlers;
using MultipleHttpClient.Application.Interfaces.Admin;
using MultipleHttpClient.Application.Interfaces.Dossier;
using MultipleHttpClient.Application.Interfaces.Reference;
using MultipleHttpClient.Application.Interfaces.Security;
using MultipleHttpClient.Application.Interfaces.User;
using MultipleHttpClient.Application.Services.Admin;
using MultipleHttpClient.Application.Services.Dossier;
using MultipleHttpClient.Application.Services.Reference;
using MultipleHttpClient.Application.Services.Security;
using MultipleHttpClient.Application.Services.User;
using MultipleHttpClient.Application.Users.Handlers;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        // Existing service registrations
        services.AddSingleton<IHttpManagementAglou, HttpManagementAglouService>();
        services.AddSingleton<IHttpUserAglou, HttpUserAglouService>();
        services.AddSingleton<IUserAglouService, UserAglouService>();
        // services.AddSingleton<IIdMappingService, IdMappingService>();
        services.AddSingleton<IHttpReferenceAglouDataService, HttpReferenceAglouDataService>();
        // services.AddSingleton<IReferenceDataMappingService, ReferenceDataMappingService>();
        services.AddSingleton<IReferenceAglouDataService, ReferenceAglouDataService>();
        services.AddSingleton<IHttpDossierAglouService, HttpDossierAglouService>();
        services.AddSingleton<IDossierAglouService, DossierAglouService>();

        // ADD ADMIN SERVICE REGISTRATION
        services.AddSingleton<IHttpAdminService, HttpAdminService>();

        // MediatR registrations
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<LoginCommandHandler>());
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<GetAllCommentsQueryHandler>());
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<GetAllCitiesQueryHandler>());

        // Validation pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddSingleton<IAccountLockoutService, AccountLockoutService>();
        // Rate limiting
        services.AddRateLimiter(options =>
        {
            options.AddPolicy("login", context =>
            RateLimitPartition.GetSlidingWindowLimiter(partitionKey: context.Request.Headers["X-Forwarded-For"], factory: _ => new SlidingWindowRateLimiterOptions()
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1)
            }));
            options.AddPolicy("passwordReset", context => RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: GetClientIp(context),
            factory: _ => new SlidingWindowRateLimiterOptions()
            {
                PermitLimit = 3,
                Window = TimeSpan.FromMinutes(60),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
        });

        // Enhanced JWT configuration
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<IJwtService, JwtService>(); // Scoped for async operations

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings.Secret)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = "role"
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetService<ILogger<JwtBearerHandler>>();
                    logger?.LogWarning("JWT Authentication failed: {0}", context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetService<ILogger<JwtBearerHandler>>();
                    logger?.LogInformation("JWT Token validated for user: {0}",
                        context.Principal?.FindFirst("user_id")?.Value);
                    return Task.CompletedTask;
                }
            };
        });

        // Enhanced authorization with role-based policies
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireClaim("internal_profile_id", "1"));
            options.AddPolicy("AdminOrRegional", policy =>
                policy.RequireClaim("internal_profile_id", "1", "2"));
            options.AddPolicy("AllUsers", policy =>
                policy.RequireClaim("internal_profile_id", "1", "2", "3"));
            options.AddPolicy("ActiveUsersOnly", policy =>
                policy.RequireClaim("is_active", "True"));
        });

        // Adding Memory cache
        services.AddMemoryCache(options =>
        {
            options.SizeLimit = configuration.GetValue<int>("MappingService:MaxCacheSize", 10000);
        });
        services.AddSingleton<IIdMappingService, DeterministicIdMappingService>();
        services.AddSingleton<IReferenceDataMappingService, DeterministicReferenceDataMappingService>();

        services.AddHealthChecks().AddCheck<MappingHealthCheck>("mapping-service");
        return services;
    }
    public static IServiceCollection AddResponseCompressionConfiguration(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
            {
                    "application/json",
                    "application/json; charset=utf-8",
                    "text/json"
            });
        });

        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });

        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });

        return services;
    }
    public static IApplicationBuilder UseCustomSecurityHeaders(this IApplicationBuilder builder)
    {
        // Health Check endpoint

        return builder.Use(async (context, next) =>
        {
            context.Response.Headers["Content-Security-Policy"] = "default-src 'self'";
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["Strict-Transport-Security"] = "max-age=63072000";
            await next();
        });
    }
    private static string GetClientIp(HttpContext context)
    {
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].ToString();
        return !string.IsNullOrEmpty(forwardedFor) ? forwardedFor : context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}