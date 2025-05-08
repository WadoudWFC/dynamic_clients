namespace MultipleHtppClient.Infrastructure;

public class ApiConfig
{
    public required string Name { get; init; }
    public required string BaseUrl { get; init; }
    public int TimeoutSeconds { get; init; } = 30;
    public ApiAuthConfig? AuthConfig { get; init; }
    public Dictionary<string, string> DefaultHeaders { get; init; } = new Dictionary<string, string>();
}
