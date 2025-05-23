
using System.Reflection.Metadata.Ecma335;
using MultipleHtppClient.Infrastructure.HTTP.Enums;
using MultipleHtppClient.Infrastructure.HTTP.Interfaces;

namespace MultipleHtppClient.Infrastructure.HTTP.Auth;

public sealed class ApiKeyAuthHandler : IAuthenticationHandler
{
    public Task AuthenticateAsync(HttpClient client, ApiAuthConfig apiAuthConfig)
    {
        if (apiAuthConfig.Parameters.TryGetValue("HeaderValue", out var apikey) && apiAuthConfig.Parameters.TryGetValue("HeaderName", out var headerName))
        {
            client.DefaultRequestHeaders.Add(headerName, apikey);

        }
        return Task.CompletedTask;
    }
    public bool CanHandle(AuthenticationType authenticationType) => authenticationType == AuthenticationType.ApiKey;
}
