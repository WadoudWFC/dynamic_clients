using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MultipleHtppClient.Domain;
using MutipleHttpClient.Domain;

namespace MultipleHtppClient.Infrastructure;

public class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HttpClientService> _logger;
    private readonly IEnumerable<IAuthenticationHandler> _authenticationHandlers;
    private readonly Configuration _configuration;
    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        Converters = {
            new StringToBoolConverter(),
            new StringToDateTimeConverter(),
            new StringToIntConverter()
        }
    };
    public HttpClientService(IHttpClientFactory httpClientFactory, ILogger<HttpClientService> logger, IEnumerable<IAuthenticationHandler> authenticationHandlers, IOptions<Configuration> configurtions)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _authenticationHandlers = authenticationHandlers;
        _configuration = configurtions.Value;
    }
    public async Task<ApiResponse<TResponse>> SendAsync<TRequest, TResponse>(ApiRequest<TRequest> apiRequest, CancellationToken cancellationToken = default)
    {
        var apiName = apiRequest.ApiName ?? _configuration.DefaultApiName;
        var apiConfig = _configuration.Apis.FirstOrDefault(a => a.Name == apiName) ?? throw new ArgumentException($"No API configuration found for {apiName}");
        using (var client = _httpClientFactory.CreateClient(apiName))
        {
            // Add ApiKey header if required
            // if (apiConfig.AuthConfig?.AuthType == AuthenticationType.ApiKey && apiConfig.AuthConfig.Parameters != null)
            // {
            //     var headerName = apiConfig.AuthConfig.Parameters["HeaderName"];
            //     var headerValue = apiConfig.AuthConfig.Parameters["HeaderValue"];
            //     if (!client.DefaultRequestHeaders.Contains(headerName))
            //     {
            //         client.DefaultRequestHeaders.Add(headerName, headerValue);
            //     }
            // }
            using (var requestMessage = CreateHttpMessage(apiRequest, apiConfig))
            {
                try
                {
                    await ApplyAuthentication(client, apiConfig);
                    ApplyHeaders(client, apiConfig, apiRequest);
                    _logger.LogDebug("Sending {0} request to {1}: {2}", apiRequest.Method, apiRequest.ApiName, apiRequest.Endpoint);
                    var response = await client.SendAsync(requestMessage, cancellationToken);
                    return await ProcessResponse<TResponse>(response);
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "Failed to call {0} at {1}", apiName, apiRequest.Endpoint);
                    return ApiResponse<TResponse>.Error(ex.Message);
                }
            }
        }
    }

    private static HttpRequestMessage CreateHttpMessage<TRequest>(ApiRequest<TRequest> apiRequest, ApiConfig apiConfig)
    {
        var requestMessage = new HttpRequestMessage(apiRequest.Method, BuildEndpointUrl(apiConfig.BaseUrl, apiRequest.Endpoint));
        if (apiRequest.Method != HttpMethod.Get && apiRequest.Data != null)
        {
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(apiRequest.Data), Encoding.UTF8, "application/json");
        }
        return requestMessage;
    }
    private static Uri BuildEndpointUrl(string BaseUrl, string endpoint)
    {
        return new Uri(new Uri(BaseUrl), endpoint);
    }

    private async Task ApplyAuthentication(HttpClient client, ApiConfig apiConfig)
    {
        if (apiConfig.AuthConfig is null || apiConfig.AuthConfig.AuthType == AuthenticationType.None) return;
        var handler = _authenticationHandlers.FirstOrDefault(ah => ah.CanHandle(apiConfig.AuthConfig.AuthType));
        if (handler is null) throw new InvalidOperationException($"No Authentication handler found for {apiConfig.AuthConfig.AuthType}");
        await handler.AuthenticateAsync(client, apiConfig.AuthConfig);
    }
    private static void ApplyHeaders<TRequest>(HttpClient client, ApiConfig apiConfig, ApiRequest<TRequest> apiRequest)
    {
        foreach (var header in apiConfig.DefaultHeaders)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }
        foreach (var header in apiRequest.Headers)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }
    }
    private static async Task<ApiResponse<TResponse>> ProcessResponse<TResponse>(HttpResponseMessage httpResponseMessage)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        if (!httpResponseMessage.IsSuccessStatusCode) return ApiResponse<TResponse>.Error(content, httpResponseMessage.StatusCode);
        try
        {
            var rawJson = JsonSerializer.Deserialize<string>(content, _jsonOptions);
            var data = JsonSerializer.Deserialize<TResponse>(rawJson, _jsonOptions);
            return ApiResponse<TResponse>.Success(data, httpResponseMessage.StatusCode);
        }
        catch (JsonException ex)
        {
            return ApiResponse<TResponse>.Error($"Failed to deserialize response: {ex.Message}\nResponse: {content}", httpResponseMessage.StatusCode);
        }
    }
}
