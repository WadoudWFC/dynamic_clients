using MultipleHtppClient.Infrastructure.HTTP.Auth;
using MultipleHtppClient.Infrastructure.HTTP.Enums;

namespace MultipleHtppClient.Infrastructure.HTTP.Interfaces;

public interface IAuthenticationHandler
{
    bool CanHandle(AuthenticationType authenticationType);
    Task AuthenticateAsync(HttpClient client, ApiAuthConfig apiAuthConfig);
}
