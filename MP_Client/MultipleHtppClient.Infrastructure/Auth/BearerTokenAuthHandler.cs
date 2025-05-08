
namespace MultipleHtppClient.Infrastructure;

public class BearerTokenAuthHandler : IAuthenticationHandler
{
    public Task AuthenticateAsync(HttpClient client, ApiAuthConfig apiAuthConfig)
    {
        if (apiAuthConfig.Parameters.TryGetValue("token", out var token))
        {
            // client.DefaultRequestHeaders.Add("Bearer", token);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        return Task.CompletedTask;
    }

    public bool CanHandle(AuthenticationType authenticationType) => authenticationType == AuthenticationType.BearerToken;
}
