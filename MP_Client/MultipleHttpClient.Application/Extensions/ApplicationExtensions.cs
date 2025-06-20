using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultipleHttpClient.Application.Commons.Behavior;
using MultipleHttpClient.Application.Dossier.Handlers;
using MultipleHttpClient.Application.Interfaces.Reference;
using MultipleHttpClient.Application.Interfaces.User;
using MultipleHttpClient.Application.Services.Reference;
using MultipleHttpClient.Application.Services.User;
using MultipleHttpClient.Application.Users.Handlers;
using MutipleHttpClient.Domain;
using FluentValidation;
using MultipleHttpClient.Application.Interfaces.Dossier;
using MultipleHttpClient.Application.Services.Dossier;
using Microsoft.AspNetCore.Builder;
using System.Threading.RateLimiting;
using System.ComponentModel.DataAnnotations;

namespace MultipleHttpClient.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHttpManagementAglou, HttpManagementAglouService>();
        services.AddSingleton<IHttpUserAglou, HttpUserAglouService>();
        services.AddSingleton<IUserAglouService, UserAglouService>();
        services.AddSingleton<IIdMappingService, IdMappingService>();
        services.AddSingleton<IHttpReferenceAglouDataService, HttpReferenceAglouDataService>();
        services.AddSingleton<IReferenceDataMappingService, ReferenceDataMappingService>();
        services.AddSingleton<IReferenceAglouDataService, ReferenceAglouDataService>();
        services.AddSingleton<IHttpDossierAglouService, HttpDossierAglouService>();
        services.AddSingleton<IDossierAglouService, DossierAglouService>();
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<LoginCommandHandler>());
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<GetAllCommentsQueryHandler>());
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<GetAllCommentQueryValidator>());
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<GetAllCitiesQueryHandler>());
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<GetAllCitiesQueryHandler>());
        // Add Dossier Handlers here
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddRateLimiter(options =>
        {
            options.AddPolicy("login", context =>
            RateLimitPartition.GetSlidingWindowLimiter(partitionKey: context.Request.Headers["X-Forwarded-For"], factory: _ => new SlidingWindowRateLimiterOptions()
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1)
            }));
        });

        return services;
    }
    public static IApplicationBuilder UseCustomSecurityHeaders(this IApplicationBuilder builder)
    {
        return builder.Use(async (context, next) =>
        {
            context.Response.Headers["Content-Security-Policy"] = "default-src 'self'";
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["Strict-Transport-Security"] = "max-age=63072000";
            await next();
        });
    }
}
