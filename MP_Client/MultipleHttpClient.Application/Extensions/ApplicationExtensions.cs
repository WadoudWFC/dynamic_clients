using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MultipleHttpClient.Application.Commons.Behavior;
using MultipleHttpClient.Application.Dossier.Handlers;
using MultipleHttpClient.Application.Interfaces.Reference;
using MultipleHttpClient.Application.Interfaces.User;
using MultipleHttpClient.Application.Services.Reference;
using MultipleHttpClient.Application.Services.User;
using MultipleHttpClient.Application.Users.Handlers;
using MutipleHttpClient.Domain;
using FluentValidation;

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
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<LoginCommandHandler>());
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<GetAllCommentsQueryHandler>());
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<GetAllCommentQueryValidator>());
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<GetAllCitiesQueryHandler>());
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<GetAllCitiesQueryHandler>());
        // Add Dossier Handlers here
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
                ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Secret")))
            };
        });
        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy(SecurityConstants.AdminOnlyPolicy, policy => policy.Requirements.Add(new ProfileAccessRequirement(ProfileType.Admin))
        //    options.AddPolicy(SecurityConstants.DossierOwnerPolicy,
        //policy => policy.Requirements
        //    .Add(new ProfileAccessRequirement(ProfileType.Admin, ProfileType.Manager))
        //    .Add(new DossierOwnerRequirement()))
        //}) // 
        return services;
    }
}
