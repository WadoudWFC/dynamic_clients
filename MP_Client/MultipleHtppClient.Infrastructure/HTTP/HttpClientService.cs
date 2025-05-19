using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MultipleHtppClient.Infrastructure.Models.Gestion.Responses;
using MutipleHttpClient.Domain.Converters.Types;

namespace MultipleHtppClient.Infrastructure;

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
        using (var client = _httpClientFactory.CreateClient(apiName))
        {
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
                    var response = await client.SendAsync(requestMessage, cancellationToken);
                    if (apiRequest.IsNestedFormat)
                    {
                        return await ProcessResponseAsync<TResponse>(response);
                    }
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
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
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
    private static LoadDossierResponse? GetLoadDossierResponseAsync(ApiResponse<string> apiResponse)
    {
        if (!string.IsNullOrEmpty(apiResponse.Data))
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var loadDossierResp = JsonSerializer.Deserialize<LoadDossierResponse>(apiResponse.Data, options);
            return loadDossierResp;
        }
        return null;
    }
    private static async Task<ApiResponse<TResponse>> ProcessResponseAsync<TResponse>(HttpResponseMessage httpResponseMessage)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync();

        if (!httpResponseMessage.IsSuccessStatusCode)
            return ApiResponse<TResponse>.Error(content, httpResponseMessage.StatusCode);

        try
        {
            // First deserialize into the Aglou10001Response wrapper
            var outerResponse = JsonSerializer.Deserialize<Aglou10001Response<JsonElement>>(content, _jsonOptions);

            if (outerResponse == null)
                return ApiResponse<TResponse>.Error("Failed to deserialize outer response", httpResponseMessage.StatusCode);

            // Handle three possible data formats:
            // 1. Data is a JSON string (your current case)
            // 2. Data is already a JSON object
            // 3. Data is null
            if (outerResponse.Data.ValueKind == JsonValueKind.String)
            {
                // Case 1: Data is a JSON string containing JSON
                var innerJson = outerResponse.Data.GetString();
                if (string.IsNullOrEmpty(innerJson))
                    return ApiResponse<TResponse>.Error("Empty data content", httpResponseMessage.StatusCode);

                var data = JsonSerializer.Deserialize<TResponse>(innerJson, _jsonOptions);
                return ApiResponse<TResponse>.Success(data, httpResponseMessage.StatusCode);
            }
            else if (outerResponse.Data.ValueKind == JsonValueKind.Object)
            {
                // Case 2: Data is already a JSON object
                var data = outerResponse.Data.Deserialize<TResponse>(_jsonOptions);
                return ApiResponse<TResponse>.Success(data, httpResponseMessage.StatusCode);
            }

            return ApiResponse<TResponse>.Error("Unsupported data format", httpResponseMessage.StatusCode);
        }
        catch (JsonException ex)
        {
            return ApiResponse<TResponse>.Error(
                $"JSON Error: {ex.Message}\nPath: {ex.Path}\nResponse: {content}",
                httpResponseMessage.StatusCode);
        }
        catch (Exception ex)
        {
            return ApiResponse<TResponse>.Error(
                $"Unexpected error: {ex.Message}\nResponse: {content}",
                httpResponseMessage.StatusCode);
        }
    }

    // Notes!!: You can Remove the ProcessResponseAsync and go back to the previous implementation 
    private static async Task<ApiResponse<TResponse>> ProcessResponseAsyncDepre<TResponse>(HttpResponseMessage httpResponseMessage)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync();

        if (!httpResponseMessage.IsSuccessStatusCode)
            return ApiResponse<TResponse>.Error(content, httpResponseMessage.StatusCode);

        try
        {
            var rawJson = JsonSerializer.Deserialize<string>(content, _jsonOptions);
            var tt = JsonSerializer.Serialize<object>(rawJson);
            var data1 = JsonSerializer.Deserialize<string>(tt, _jsonOptions);
            var rawJson1 = JsonSerializer.Deserialize<TResponse>(data1, _jsonOptions);

            // First try to parse as direct object (for simple endpoints) 
            try
            {
                using var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;
                Tuple<string, string> obj = new Tuple<string, string>("codeReponse", "data");
                // Check if this looks like an Aglou response
                if (root.TryGetProperty(obj.Item1, out _) && root.TryGetProperty(obj.Item2, out var dataProp))
                {
                    // Handle case where data is a JSON string
                    if (dataProp.ValueKind == JsonValueKind.String)
                    {
                        var jsonString = dataProp.GetString();
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            // Clean the JSON string
                            var cleanJson = jsonString.StartsWith("\"")
                                ? jsonString.Trim('"').Replace("\\\"", "\"")
                                : jsonString;

                            var data = JsonSerializer.Deserialize<TResponse>(cleanJson, _jsonOptions);
                            if (data != null)
                            {
                                return ApiResponse<TResponse>.Success(data, httpResponseMessage.StatusCode);
                            }
                        }
                    }
                    // Handle case where data is a direct object
                    else
                    {
                        var data = JsonSerializer.Deserialize<TResponse>(dataProp.GetRawText(), _jsonOptions);
                        if (data != null)
                        {
                            return ApiResponse<TResponse>.Success(data, httpResponseMessage.StatusCode);
                        }
                    }
                }
            }
            catch (JsonException ex)
            {
                return ApiResponse<TResponse>.Error(
                    $"Failed to deserialize Aglou response: {ex.Message}\nResponse: {content}",
                    httpResponseMessage.StatusCode);
            }

            // Final fallback - try direct deserialization again
            try
            {
                var finalAttempt = JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
                if (finalAttempt != null)
                {
                    return ApiResponse<TResponse>.Success(finalAttempt, httpResponseMessage.StatusCode);
                }
            }
            catch { /* Ignore and return error */ }

            return ApiResponse<TResponse>.Error("Unable to determine response format", httpResponseMessage.StatusCode);
        }
        catch (Exception ex)
        {
            return ApiResponse<TResponse>.Error(
                $"Unexpected error: {ex.Message}\nResponse: {content}",
                httpResponseMessage.StatusCode);
        }
    }

    private enum ResponseType
    {
        DirectObject,
        NestedJsonString,
        AglouWrapper,
        Unknown
    }

    private static ResponseType DetectResponseType(string jsonContent)
    {
        try
        {
            using var doc = JsonDocument.Parse(jsonContent);
            var root = doc.RootElement;

            // Check for Aglou wrapper structure
            if (root.TryGetProperty("codeReponse", out _) &&
                root.TryGetProperty("data", out var dataProp))
            {
                return dataProp.ValueKind == JsonValueKind.String
                    ? ResponseType.NestedJsonString
                    : ResponseType.AglouWrapper;
            }

            // Otherwise assume it's a direct object
            return ResponseType.DirectObject;
        }
        catch
        {
            return ResponseType.Unknown;
        }
    }

    private static ApiResponse<TResponse> HandleDirectResponse<TResponse>(string jsonContent, HttpStatusCode statusCode)
    {
        try
        {
            var result = JsonSerializer.Deserialize<TResponse>(jsonContent, _jsonOptions);
            return result != null
                ? ApiResponse<TResponse>.Success(result, statusCode)
                : ApiResponse<TResponse>.Error("Deserialized null result", statusCode);
        }
        catch (JsonException ex)
        {
            return ApiResponse<TResponse>.Error(
                $"Failed to deserialize direct response: {ex.Message}",
                statusCode);
        }
    }

    private static ApiResponse<TResponse> HandleNestedJsonResponse<TResponse>(string jsonContent, HttpStatusCode statusCode)
    {
        try
        {
            var wrapper = JsonSerializer.Deserialize<Aglou10001Response<string>>(jsonContent, _jsonOptions);
            if (wrapper?.Data == null)
                return ApiResponse<TResponse>.Error("Empty nested JSON data", statusCode);

            var cleanJson = wrapper.Data.StartsWith("\"")
                ? wrapper.Data.Trim('"').Replace("\\\"", "\"")
                : wrapper.Data;

            var data = JsonSerializer.Deserialize<TResponse>(cleanJson, _jsonOptions);
            return data != null
                ? ApiResponse<TResponse>.Success(data, statusCode)
                : ApiResponse<TResponse>.Error("Failed to deserialize nested content", statusCode);
        }
        catch (JsonException ex)
        {
            return ApiResponse<TResponse>.Error(
                $"Failed to deserialize nested response: {ex.Message}",
                statusCode);
        }
    }

    private static ApiResponse<TResponse> HandleAglouWrappedResponse<TResponse>(string jsonContent, HttpStatusCode statusCode)
    {
        try
        {
            var response = JsonSerializer.Deserialize<Aglou10001Response<TResponse>>(jsonContent, _jsonOptions);
            return response.Data != null
                ? ApiResponse<TResponse>.Success(response.Data, statusCode)
                : ApiResponse<TResponse>.Error("Empty wrapped response data", statusCode);
        }
        catch (JsonException ex)
        {
            return ApiResponse<TResponse>.Error(
                $"Failed to deserialize wrapped response: {ex.Message}",
                statusCode);
        }
    }
}
