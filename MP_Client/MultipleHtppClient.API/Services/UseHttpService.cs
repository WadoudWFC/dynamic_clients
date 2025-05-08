using Microsoft.AspNetCore.Mvc;
using MultipleHtppClient.Infrastructure;

namespace MultipleHtppClient.API;

public class UseHttpService : IUseHttpService
{
    private readonly IHttpClientService _clientService;
    public UseHttpService(IHttpClientService clientService) => _clientService = clientService;

    public async Task<ApiResponse<Aglou10001Response<AglouUser>>> CanTryLoginAsync(CanTryLoginRequestBody email)
    {
        ApiRequest<CanTryLoginRequestBody> request = new ApiRequest<CanTryLoginRequestBody>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/utilisateur/Cantrylogin",
            Method = HttpMethod.Post,
            Data = email
        };
        return await _clientService.SendAsync<CanTryLoginRequestBody, Aglou10001Response<AglouUser>>(request);
    }

    public async Task<ApiResponse<IEnumerable<Product>>> GetAllProductsAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "rest-dev-api",
            Endpoint = "/objects",
            Method = HttpMethod.Get
        };
        return await _clientService.SendAsync<object, IEnumerable<Product>>(request);
    }

    public async Task<ApiResponse<Product>> GetProductByIdAsync(int id)
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "rest-dev-api",
            Endpoint = $"/objects/{id}",
            Method = HttpMethod.Get
        };
        return await _clientService.SendAsync<object, Product>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<AglouLoginResponse>>> LoginAsync(LoginRequestBody loginRequestBody)
    {
        ApiRequest<LoginRequestBody> request = new ApiRequest<LoginRequestBody>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/utilisateur/Login",
            Method = HttpMethod.Post,
            Data = loginRequestBody
        };
        return await _clientService.SendAsync<LoginRequestBody, Aglou10001Response<AglouLoginResponse>>(request);
    }

    public async Task<ApiResponse<object>> TickerAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "aqlou-q-client",
            Endpoint = "/",
            Method = HttpMethod.Get
        };
        return await _clientService.SendAsync<object, object>(request);
    }

    
}
