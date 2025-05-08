namespace MultipleHtppClient.Infrastructure;

public class ApiRequest<TRequest>
{
    public required string ApiName { get; init; }
    public required string Endpoint { get; init; }
    public HttpMethod Method { get; init; } = HttpMethod.Get;
    public TRequest? Data { get; init; }
    public Dictionary<string, string> Headers { get; init; } = new Dictionary<string, string>();
}
