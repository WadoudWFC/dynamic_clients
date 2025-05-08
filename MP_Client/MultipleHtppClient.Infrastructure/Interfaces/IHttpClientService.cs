namespace MultipleHtppClient.Infrastructure;

public interface IHttpClientService
{
    Task<ApiResponse<TResponse>> SendAsync<TRequest, TResponse>(ApiRequest<TRequest> apiRequest, CancellationToken cancellationToken = default);
}
