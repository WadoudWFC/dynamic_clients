using MultipleHtppClient.Infrastructure.HTTP.Interfaces;

namespace MultipleHtppClient.Infrastructure.HTTP.Auth;

public class TokenManager : ITokenManager
{
    private string? _token;
    public string? GetToken() => _token;

    public void SetToken(string token) => _token = token;
}
