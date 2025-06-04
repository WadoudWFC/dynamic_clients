using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MultipleHttpClient.Application.Commons.Behavior;
using MultipleHttpClient.Application.Interfaces.Reference;
using MultipleHttpClient.Application.Interfaces.User;
using MultipleHttpClient.Application.Services.Reference;
using MultipleHttpClient.Application.Services.User;
using MultipleHttpClient.Application.Users.Handlers;
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
        services.AddSingleton<IHttpReferenceAglouDataService, HttpReferenceAglouDataService>();
        services.AddSingleton<IReferenceDataMappingService, ReferenceDataMappingService>();
        services.AddSingleton<IReferenceAglouDataService, ReferenceAglouDataService>();
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<LoginCommandHandler>());
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<GetAllCitiesQueryHandler>());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}
