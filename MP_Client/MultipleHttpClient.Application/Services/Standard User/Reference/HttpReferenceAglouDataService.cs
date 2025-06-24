using MultipleHtppClient.Infrastructure;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Commercial_Activities.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Demand_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Gepgraphical_Information.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Partner_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Property_Information.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;
using MultipleHtppClient.Infrastructure.HTTP.Interfaces;
using MultipleHtppClient.Infrastructure.HTTP.REST;
using MultipleHttpClient.Application.Interfaces.Reference;

namespace MultipleHttpClient.Application.Services.Reference;

public class HttpReferenceAglouDataService : IHttpReferenceAglouDataService
{
    #region Constants
    const string monopp_extern = "aglou-q-monopp-extern";
    #endregion
    private readonly IHttpClientService _httpClientService;
    public HttpReferenceAglouDataService(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }
    #region Methods
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<ActivityNatureResponse>>>> GetAllActivitiesAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/natureactivite/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = new { }
        };
        return await _httpClientService.SendAsync<object, Aglou10001Response<IEnumerable<ActivityNatureResponse>>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<VilleResponse>>>> GetAllCitiesAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/ville/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = new { }
        };
        return await _httpClientService.SendAsync<object, Aglou10001Response<IEnumerable<VilleResponse>>>(request);

    }
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<PackResponse>>>> GetAllPacksAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/pack/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = new { }
        };
        return await _httpClientService.SendAsync<object, Aglou10001Response<IEnumerable<PackResponse>>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<RegionResponse>>>> GetAllRegionsAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/region/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = new { }
        };
        return await _httpClientService.SendAsync<object, Aglou10001Response<IEnumerable<RegionResponse>>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<ArrondissementResponse>>>> GetArrondissementsAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/arrondissement/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = new { }
        };
        return await _httpClientService.SendAsync<object, Aglou10001Response<IEnumerable<ArrondissementResponse>>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<CommercialCutting>>>> GetCommercialCuttingAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/decoupagecommercial/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true
        };
        return await _httpClientService.SendAsync<object, Aglou10001Response<IEnumerable<CommercialCutting>>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<DemandType>>>> GetDemandsTypeAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/typedemende/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true
        };
        return await _httpClientService.SendAsync<object, Aglou10001Response<IEnumerable<DemandType>>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<PartnersType>>>> GetPartnerTypesAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/typepartenaire/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true
        };
        return await _httpClientService.SendAsync<object, Aglou10001Response<IEnumerable<PartnersType>>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<TypeBienResponse>>>> GetTypeBienAsync()
    {
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/typebien/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = new { }
        };
        return await _httpClientService.SendAsync<object, Aglou10001Response<IEnumerable<TypeBienResponse>>>(request);
    }
    #endregion
}
