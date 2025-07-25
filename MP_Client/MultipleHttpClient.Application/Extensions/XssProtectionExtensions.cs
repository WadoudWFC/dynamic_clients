using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MultipleHttpClient.Application;

public static class XssProtectionExtensions
{
    public static IServiceCollection AddXssProtection(this IServiceCollection services)
    {
        services.AddSingleton<IInputSanitizationService, InputSanitizationService>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(XssProtectionBehavior<,>));

        return services;
    }

    public static IApplicationBuilder UseXssProtection(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<XssProtectionMiddleware>();
    }
}
