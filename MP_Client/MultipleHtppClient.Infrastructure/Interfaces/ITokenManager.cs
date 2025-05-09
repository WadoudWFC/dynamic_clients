namespace MultipleHtppClient.Infrastructure;

public interface ITokenManager
{
    string? GetToken();
    void SetToken(string token);
}
