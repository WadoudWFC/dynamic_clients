using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultipleHtppClient.Infrastructure.HTTP.Auth;
using MultipleHtppClient.Infrastructure.HTTP.Configurations;
using MultipleHtppClient.Infrastructure.HTTP.REST;
using MultipleHtppClient.Infrastructure.HTTP.Interfaces;

namespace MultipleHtppClient.Infrastructure.HTTP.Extensions;

public static class HttpClientExtensions
{
    public static IServiceCollection AddApiHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Configuration>(configuration.GetSection("ApiConfigurations"));
        var apiConfig = configuration.GetSection("ApiConfigurations").Get<Configuration>() ?? throw new InvalidOperationException("Missing API configurations");
        // Register each configured API Client
        foreach (var api in apiConfig.Apis)
        {
            services.AddHttpClient(api.Name, client =>
            {
                client.BaseAddress = new Uri(api.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(api.TimeoutSeconds);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                // Add security headers
                client.DefaultRequestHeaders.Add("X-Content-Type-Options", "nosniff");
                client.DefaultRequestHeaders.Add("X-Frame-Options", "DENY");
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                // To be removed in production mode
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });
            // Register authentication headers
            services.AddSingleton<IAuthenticationHandler, ApiKeyAuthHandler>();
            services.AddSingleton<IAuthenticationHandler, BearerTokenAuthHandler>();
            services.AddSingleton<ITokenManager, TokenManager>();
            // Register main service
            services.AddSingleton<IHttpClientService, HttpClientService>();
        }
        return services;
    }
}
