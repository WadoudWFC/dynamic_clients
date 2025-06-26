using System.Text.Json;
using MultipleHtppClient.Infrastructure;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.User_Account.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.User_Account.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Models.User_Account.Responses;
using MultipleHtppClient.Infrastructure.HTTP.Interfaces;
using MultipleHtppClient.Infrastructure.HTTP.REST;
using MultipleHttpClient.Application.Interfaces.User;

namespace MultipleHttpClient.Application.Services.User;

public class HttpUserAglouService : IHttpUserAglou
{
    const string monopp_extern = "aglou-q-monopp-extern";
    private readonly IHttpClientService _clientService;
    private readonly ITokenManager _tokenManager;
    public HttpUserAglouService(IHttpClientService clientService, ITokenManager tokenManager)
    {
        _clientService = clientService;
        _tokenManager = tokenManager;
    }
    public async Task<ApiResponse<Aglou10001Response<AglouUser>>> CanTryLoginAsync(CanTryLoginRequestBody email)
    {
        ApiRequest<CanTryLoginRequestBody> request = new ApiRequest<CanTryLoginRequestBody>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/utilisateur/Cantrylogin",
            Method = HttpMethod.Post,
            Data = email,
        };
        return await _clientService.SendAsync<CanTryLoginRequestBody, Aglou10001Response<AglouUser>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<object>>> ForgetPasswordAsync(ForgetPasswordRequestBody forgetPasswordRequestBody)
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/utilisateur/PasswordForgotten",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = false,
            Data = forgetPasswordRequestBody
        };
        return await _clientService.SendAsync<object, Aglou10001Response<object>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<GetAllUsersResponse>>>> GetAllUsersAsync(GetAllUsersRequestBody getAllUsersRequestBody)
    {
        ApiRequest<GetAllUsersRequestBody> request = new ApiRequest<GetAllUsersRequestBody>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/utilisateur/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = getAllUsersRequestBody
        };
        return await _clientService.SendAsync<GetAllUsersRequestBody, Aglou10001Response<IEnumerable<GetAllUsersResponse>>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<LoadUserResponse>>> LoadUserAsync(LogoutRequestBody logoutRequestBody)
    {
        var request = new ApiRequest<LogoutRequestBody>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/utilisateur/Load",
            Method = HttpMethod.Post,
            Data = logoutRequestBody,
            RequiresApiKey = true,
            RequiresBearerToken = true
        };

        return await _clientService.SendNestedJsonAsync<LogoutRequestBody, LoadUserResponse>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<AglouLoginResponse>>> LoginAsync(LoginRequestBody loginRequestBody)
    {
        ApiRequest<LoginRequestBody> request = new ApiRequest<LoginRequestBody>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/utilisateur/Login",
            Method = HttpMethod.Post,
            Data = loginRequestBody,
            RequiresApiKey = true,
            RequiresBearerToken = false
        };
        var response = await _clientService.SendAsync<LoginRequestBody, Aglou10001Response<AglouLoginResponse>>(request);
        if (response.IsSuccess && response.Data?.Data?.BearerKey != null)
        {
            _tokenManager.SetToken(response.Data.Data.BearerKey);
        }
        return response;
    }
    public async Task<ApiResponse<Aglou10001Response<object>>> LogoutAsync(LogoutRequestBody logoutRequestBody)
    {
        ApiRequest<LogoutRequestBody> request = new ApiRequest<LogoutRequestBody>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/utilisateur/Logout",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = logoutRequestBody
        };
        return await _clientService.SendAsync<LogoutRequestBody, Aglou10001Response<object>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<object>>> RegisterUserAsync(RegisterUserRequestBody registerUserRequestBody)
    {
        ApiRequest<RegisterUserRequestBody> request = new ApiRequest<RegisterUserRequestBody>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/utilisateur/InsertUser",
            Method = HttpMethod.Post,
            RequiresApiKey = false,
            Data = registerUserRequestBody
        };
        return await _clientService.SendAsync<RegisterUserRequestBody, Aglou10001Response<object>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<object>>> UpdatePasswordAsync(UpdatePasswordRequestBody updatePasswordRequestBody)
    {
        ApiRequest<UpdatePasswordRequestBody> request = new ApiRequest<UpdatePasswordRequestBody>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/utilisateur/UpdatePassWord",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = false,
            Data = updatePasswordRequestBody
        };
        return await _clientService.SendAsync<UpdatePasswordRequestBody, Aglou10001Response<object>>(request);
    }
}
