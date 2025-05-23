using MultipleHtppClient.Infrastructure.HTTP.Enums;

namespace MultipleHtppClient.Infrastructure.HTTP.Auth;

public sealed class ApiAuthConfig
{
    public AuthenticationType AuthType { get; init; } = AuthenticationType.None;
    public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
}
