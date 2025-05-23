using MultipleHtppClient.Infrastructure.HTTP.Enums;
using MultipleHtppClient.Infrastructure.HTTP.Interfaces;

namespace MultipleHtppClient.Infrastructure.HTTP.Auth;

public class BearerTokenAuthHandler : IAuthenticationHandler
{
    private readonly ITokenManager _tokenManager;
    public BearerTokenAuthHandler(ITokenManager tokenManager)
    {
        _tokenManager = tokenManager;
    }
    public Task AuthenticateAsync(HttpClient client, ApiAuthConfig apiAuthConfig)
    {
        var token = _tokenManager.GetToken();
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        return Task.CompletedTask;
    }

    public bool CanHandle(AuthenticationType authenticationType) => authenticationType == AuthenticationType.BearerToken;
}
