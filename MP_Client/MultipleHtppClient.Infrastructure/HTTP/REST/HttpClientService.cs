using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MultipleHtppClient.Infrastructure.HTTP.Configurations;
using MultipleHtppClient.Infrastructure.HTTP.Enums;
using MultipleHtppClient.Infrastructure.HTTP.Interfaces;
using MutipleHttpClient.Domain.Converters.Types;

namespace MultipleHtppClient.Infrastructure.HTTP.REST;

public class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HttpClientService> _logger;
    private readonly IEnumerable<IAuthenticationHandler> _authenticationHandlers;
    private readonly Configuration _configuration;
    private readonly ITokenManager _tokenManager;
    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        Converters = {
            new StringToBoolConverter(),
            new StringToDateTimeConverter(),
            new StringToIntConverter()
        }
    };
    public HttpClientService(IHttpClientFactory httpClientFactory, ILogger<HttpClientService> logger, IEnumerable<IAuthenticationHandler> authenticationHandlers, IOptions<Configuration> configurtions, ITokenManager tokenManager)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _authenticationHandlers = authenticationHandlers;
        _configuration = configurtions.Value;
        _tokenManager = tokenManager;
    }
    public async Task<ApiResponse<TResponse>> SendAsync<TRequest, TResponse>(ApiRequest<TRequest> apiRequest, CancellationToken cancellationToken = default)
    {
        var apiName = apiRequest.ApiName ?? _configuration.DefaultApiName;
        var apiConfig = _configuration.Apis.FirstOrDefault(a => a.Name == apiName) ?? throw new ArgumentException($"No API configuration found for {apiName}");
        var client = _httpClientFactory.CreateClient(apiName);
        using (var requestMessage = CreateHttpMessage(apiRequest, apiConfig))
        {
            try
            {
                // await ApplyAuthentication(client, apiConfig);
                if (apiRequest.RequiresApiKey)
                {
                    await ApplyApiKeyAuth(client, apiConfig);
                }
                if (apiRequest.RequiresBearerToken)
                {
                    await ApplyBearerTokenAuth(client, apiConfig);
                }
                ApplyHeaders(client, apiConfig, apiRequest);
                _logger.LogDebug("Sending {0} request to {1}: {2}", apiRequest.Method, apiRequest.ApiName, apiRequest.Endpoint);
                if (apiRequest.IsForm && apiRequest.Data != null)
                {
                    var httpRequestMessage = CreateFormHttpMessage(apiRequest, apiConfig);
                    var formResponse = await client.SendAsync(httpRequestMessage, cancellationToken);
                    return await ProcessResponseAsync<TResponse>(formResponse);
                }
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
    public async Task<ApiResponse<Aglou10001Response<TResponse>>> SendNestedJsonAsync<TRequest, TResponse>(ApiRequest<TRequest> apiRequest)
    {
        var rawResponse = await SendAsync<TRequest, Aglou10001Response<string>>(apiRequest);

        if (!rawResponse.IsSuccess || rawResponse.Data?.Data == null)
        {
            return ApiResponse<Aglou10001Response<TResponse>>.Error(
                rawResponse.ErrorMessage ?? "Failed to get response data",
                rawResponse.StatusCode);
        }

        try
        {
            // Deserialize the inner JSON
            var innerData = JsonSerializer.Deserialize<TResponse>(rawResponse.Data.Data, _jsonOptions);

            // Reconstruct the response
            var finalResponse = new Aglou10001Response<TResponse>
            {
                ResponseCode = rawResponse.Data.ResponseCode,
                Message = rawResponse.Data.Message,
                Data = innerData
            };

            return ApiResponse<Aglou10001Response<TResponse>>.Success(finalResponse, rawResponse.StatusCode);
        }
        catch (JsonException ex)
        {
            return ApiResponse<Aglou10001Response<TResponse>>.Error($"Failed to deserialize nested data: {ex.Message}", rawResponse.StatusCode);
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
    private static HttpRequestMessage CreateFormHttpMessage<TRequest>(ApiRequest<TRequest> apiRequest, ApiConfig apiConfig)
    {
        var requestMessage = new HttpRequestMessage(apiRequest.Method, BuildEndpointUrl(apiConfig.BaseUrl, apiRequest.Endpoint));
        var formContent = new MultipartFormDataContent();
        if (apiRequest.Data != null)
        {
            foreach (var prop in typeof(TRequest).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = prop.GetValue(apiRequest.Data);
                if (value is null) continue;
                if (value is string strVal)
                {
                    //formContent.Add(new StringContent(strVal), prop.Name); /*This was changed to prevent FORM XSS*/
                    formContent.Add(new StringContent(HttpUtility.HtmlEncode(strVal)), HttpUtility.HtmlEncode(prop.Name));
                }
                else if (value is byte[] byteArray)
                {
                    var byteContent = new ByteArrayContent(byteArray);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    formContent.Add(byteContent, prop.Name, "file.bin");
                }
                else
                {
                    formContent.Add(new StringContent(value.ToString()), prop.Name);
                }
            }
            requestMessage.Content = formContent;
            if (requestMessage.Content.Headers.ContentType != null)
            {
                requestMessage.Content.Headers.ContentType.MediaType = "multipart/form-data";
            }
        }
        return requestMessage;
    }

    private async Task ApplyAuthentication(HttpClient client, ApiConfig apiConfig)
    {
        if (apiConfig.AuthConfig is null || apiConfig.AuthConfig.AuthType == AuthenticationType.None) return;
        var handler = _authenticationHandlers.FirstOrDefault(ah => ah.CanHandle(apiConfig.AuthConfig.AuthType));
        if (handler is null) throw new InvalidOperationException($"No Authentication handler found for {apiConfig.AuthConfig.AuthType}");
        await handler.AuthenticateAsync(client, apiConfig.AuthConfig);
    }
    private async Task ApplyApiKeyAuth(HttpClient client, ApiConfig apiConfig)
    {
        if (apiConfig.AuthConfig?.AuthType == AuthenticationType.ApiKey)
        {
            var handler = _authenticationHandlers.FirstOrDefault(h => h.CanHandle(AuthenticationType.ApiKey));
            if (handler != null)
            {
                await handler.AuthenticateAsync(client, apiConfig.AuthConfig);
            }
        }
    }
    private Task ApplyBearerTokenAuth(HttpClient client, ApiConfig apiConfig)
    {
        var token = _tokenManager.GetToken();
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        return Task.CompletedTask;
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
    private async Task<ApiResponse<TResponse>> ProcessResponseAsync<TResponse>(HttpResponseMessage formResponse)
    {
        var content = await formResponse.Content.ReadAsStringAsync();
        if (!formResponse.IsSuccessStatusCode) return ApiResponse<TResponse>.Error(content, formResponse.StatusCode);
        try
        {
            var data = JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
            return ApiResponse<TResponse>.Success(data, formResponse.StatusCode);
        }
        catch (Exception ex)
        {
            return ApiResponse<TResponse>.Error($"Failed to deserialize response: {ex.Message}: {content}", formResponse.StatusCode);
        }
    }

}
