namespace MultipleHtppClient.Infrastructure;

public interface IAuthenticationHandler
{
    bool CanHandle(AuthenticationType authenticationType);
    Task AuthenticateAsync(HttpClient client, ApiAuthConfig apiAuthConfig);
}
