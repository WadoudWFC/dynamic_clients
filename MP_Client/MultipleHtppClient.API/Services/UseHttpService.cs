using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Tree;
using MultipleHtppClient.Infrastructure;
using MultipleHtppClient.Infrastructure.Models.Gestion.Requests;
using MultipleHtppClient.Infrastructure.Models.Gestion.Responses;

namespace MultipleHtppClient.API;

public class UseHttpService : IUseHttpService
{
    private readonly IHttpClientService _clientService;
    private readonly ITokenManager _tokenManager;
    public UseHttpService(IHttpClientService clientService, ITokenManager tokenManager)
    {
        _clientService = clientService;
        _tokenManager = tokenManager;
    }

    public async Task<ApiResponse<Aglou10001Response<AglouUser>>> CanTryLoginAsync(CanTryLoginRequestBody email)
    {
        ApiRequest<CanTryLoginRequestBody> request = new ApiRequest<CanTryLoginRequestBody>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/utilisateur/Cantrylogin",
            Method = HttpMethod.Post,
            Data = email,
        };
        return await _clientService.SendAsync<CanTryLoginRequestBody, Aglou10001Response<AglouUser>>(request);
    }

    public async Task<ApiResponse<IEnumerable<Product>>> GetAllProductsAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "rest-dev-api",
            Endpoint = "/objects",
            Method = HttpMethod.Get,
            RequiresApiKey = false
        };
        return await _clientService.SendAsync<object, IEnumerable<Product>>(request);
    }

    public async Task<ApiResponse<Product>> GetProductByIdAsync(int id)
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "rest-dev-api",
            Endpoint = $"/objects/{id}",
            Method = HttpMethod.Get,
            RequiresApiKey = false
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

    public async Task<ApiResponse<Aglou10001Response<DossierCounts>>> GetCountsAsync(string userId, string roleId)
    {
        ApiRequest<DossierCountRequest> request = new ApiRequest<DossierCountRequest>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/dossier/getCounts",
            Method = HttpMethod.Post,
            Data = new DossierCountRequest
            {
                UserId = userId,
                RoleId = roleId,
                ApplyFilter = true
            },
            RequiresApiKey = true,
            RequiresBearerToken = true
        };
        return await _clientService.SendAsync<DossierCountRequest, Aglou10001Response<DossierCounts>>(request);
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

    public async Task<ApiResponse<Aglou10001Response<IEnumerable<PartnersType>>>> GetPartnerTypes()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/typepartenaire/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true
        };
        return await _clientService.SendAsync<object, Aglou10001Response<IEnumerable<PartnersType>>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<IEnumerable<DossierStatus>>>> GetDossierStatusAsync(ProfileRoleRequestBody? profileRequestBody)
    {
        profileRequestBody.RoleId = string.Empty;
        ApiRequest<ProfileRoleRequestBody> request = new ApiRequest<ProfileRoleRequestBody>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/statutdossier/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = profileRequestBody
        };
        return await _clientService.SendAsync<ProfileRoleRequestBody, Aglou10001Response<IEnumerable<DossierStatus>>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<IEnumerable<DemandType>>>> GetDemandsTypeAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/typedemende/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true
        };
        return await _clientService.SendAsync<object, Aglou10001Response<IEnumerable<DemandType>>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<IEnumerable<CommercialCutting>>>> GetCommercialCuttingAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/decoupagecommercial/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true
        };
        return await _clientService.SendAsync<object, Aglou10001Response<IEnumerable<CommercialCutting>>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<IEnumerable<DossierAll>>>> GetAllDossierAsync(ProfileRoleRequestBody? profileRoleRequestBody)
    {
        profileRoleRequestBody.RoleId = string.Empty;
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/dossier/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = profileRoleRequestBody
        };
        return await _clientService.SendAsync<object, Aglou10001Response<IEnumerable<DossierAll>>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<IEnumerable<Partner>>>> GetPartnersAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/partenaire/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = new { }
        };
        return await _clientService.SendAsync<object, Aglou10001Response<IEnumerable<Partner>>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<IEnumerable<DossierSearchResponse>>>> SearchDossier(SearchDossierRequestBody searchDossierRequestBody)
    {
        ApiRequest<SearchDossierRequestBody> request = new ApiRequest<SearchDossierRequestBody>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/dossier/Search",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = searchDossierRequestBody
        };
        return await _clientService.SendAsync<SearchDossierRequestBody, Aglou10001Response<IEnumerable<DossierSearchResponse>>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<object>>> LogoutAsync(LogoutRequestBody logoutRequestBody)
    {
        ApiRequest<LogoutRequestBody> request = new ApiRequest<LogoutRequestBody>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/utilisateur/Logout",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = logoutRequestBody
        };
        return await _clientService.SendAsync<LogoutRequestBody, Aglou10001Response<object>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<IEnumerable<ActivityNatureResponse>>>> GetAllActivitiesAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/natureactivite/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = new { }
        };
        return await _clientService.SendAsync<object, Aglou10001Response<IEnumerable<ActivityNatureResponse>>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<IEnumerable<PackResponse>>>> GetAllPacksAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/pack/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = new { }
        };
        return await _clientService.SendAsync<object, Aglou10001Response<IEnumerable<PackResponse>>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<IEnumerable<VilleResponse>>>> GetAllCitiesAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/ville/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = new { }
        };
        return await _clientService.SendAsync<object, Aglou10001Response<IEnumerable<VilleResponse>>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<IEnumerable<ArrondissementResponse>>>> GetArrondissementsAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/arrondissement/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = new { }
        };
        return await _clientService.SendAsync<object, Aglou10001Response<IEnumerable<ArrondissementResponse>>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<IEnumerable<TypeBienResponse>>>> GetTypeBienAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/typebien/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = new { }
        };
        return await _clientService.SendAsync<object, Aglou10001Response<IEnumerable<TypeBienResponse>>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<IEnumerable<RegionResponse>>>> GetAllRegionsAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/region/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = new { }
        };
        return await _clientService.SendAsync<object, Aglou10001Response<IEnumerable<RegionResponse>>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<object>>> ForgetPasswordAsync(ForgetPasswordRequestBody forgetPasswordRequestBody)
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/utilisateur/PasswordForgotten",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = false,
            Data = forgetPasswordRequestBody
        };
        return await _clientService.SendAsync<object, Aglou10001Response<object>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<object>>> UpdatePasswordAsync(UpdatePasswordRequestBody updatePasswordRequestBody)
    {
        ApiRequest<UpdatePasswordRequestBody> request = new ApiRequest<UpdatePasswordRequestBody>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/utilisateur/UpdatePassWord",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = false,
            Data = updatePasswordRequestBody
        };
        return await _clientService.SendAsync<UpdatePasswordRequestBody, Aglou10001Response<object>>(request);
    }

    public async Task<ApiResponse<Aglou10001Response<string>>> LoadDossierAsync(LogoutRequestBody idRequestBody)
    {
        ApiRequest<LogoutRequestBody> request = new ApiRequest<LogoutRequestBody>
        {
            ApiName = "aglou-q-monopp-extern",
            Endpoint = "/api/v2/dossier/Load",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = idRequestBody,
            IsNestedFormat = false
        };
        return await _clientService.SendAsync<LogoutRequestBody, Aglou10001Response<string>>(request);
    }
}
