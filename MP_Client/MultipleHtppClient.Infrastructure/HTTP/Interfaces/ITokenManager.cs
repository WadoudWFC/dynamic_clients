namespace MultipleHtppClient.Infrastructure.HTTP.Interfaces;

public interface ITokenManager
{
    string? GetToken();
    void SetToken(string token);
}
