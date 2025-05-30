using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MultipleHttpClient.Application.Commons.Behavior;
using MultipleHttpClient.Application.Users.Validators;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddSingleton<IHttpManagementAglou, HttpManagementAglouService>();
        services.AddSingleton<IHttpUserAglou, HttpUserAglouService>();
        services.AddSingleton<IUserAglouService, UserAglouService>();
        services.AddSingleton<IIdMappingService, IdMappingService>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}
