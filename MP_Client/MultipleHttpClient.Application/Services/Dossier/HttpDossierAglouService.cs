using System.Net;
using System.Text.Json;
using MultipleHtppClient.Infrastructure;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Comment_Management.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Comment_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Demand_Management.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Demand_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Statistics_and_Counts.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.User_Account.Requests;
using MultipleHtppClient.Infrastructure.HTTP.Interfaces;
using MultipleHtppClient.Infrastructure.HTTP.REST;
using MultipleHttpClient.Application.Interfaces.Dossier;

namespace MultipleHttpClient.Application.Services.Dossier;

public class HttpDossierAglouService : IHttpDosserAglouService
{
    #region Constants
    const string monopp_extern = "aglou-q-monopp-extern";
    #endregion
    private readonly IHttpClientService _httpClientService;
    public HttpDossierAglouService(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }
    #region Methods
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<GetAllCommentsResponse>>>> GetAllCommentsAsync(GetAllCommentRequestBody getAllCommentRequestBody)
    {
        ApiRequest<GetAllCommentRequestBody> request = new ApiRequest<GetAllCommentRequestBody>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/commentaire/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = getAllCommentRequestBody
        };
        return await _httpClientService.SendAsync<GetAllCommentRequestBody, Aglou10001Response<IEnumerable<GetAllCommentsResponse>>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<DossierAll>>>> GetAllDossierAsync(ProfileRoleRequestBody? profileRoleRequestBody)
    {
        profileRoleRequestBody.RoleId = string.Empty;
        ApiRequest<object> request = new ApiRequest<object>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/dossier/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = profileRoleRequestBody
        };
        return await _httpClientService.SendAsync<object, Aglou10001Response<IEnumerable<DossierAll>>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<DossierCounts>>> GetCountsAsync(string userId, string roleId)
    {
        ApiRequest<DossierCountRequest> request = new ApiRequest<DossierCountRequest>
        {
            ApiName = monopp_extern,
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
        return await _httpClientService.SendAsync<DossierCountRequest, Aglou10001Response<DossierCounts>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<DossierStatus>>>> GetDossierStatusAsync(ProfileRoleRequestBody? profileRequestBody)
    {
        profileRequestBody.RoleId = string.Empty;
        ApiRequest<ProfileRoleRequestBody> request = new ApiRequest<ProfileRoleRequestBody>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/statutdossier/GetAll",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = profileRequestBody
        };
        return await _httpClientService.SendAsync<ProfileRoleRequestBody, Aglou10001Response<IEnumerable<DossierStatus>>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<object?>>> InsertCommentAsync(InsertCommentRequestBody insertCommentRequestBody)
    {
        ApiRequest<InsertCommentRequestBody> request = new ApiRequest<InsertCommentRequestBody>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/commentaire/Insert",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = insertCommentRequestBody
        };
        return await _httpClientService.SendAsync<InsertCommentRequestBody, Aglou10001Response<object?>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<object?>>> InsertDossierFormAsync(InsertDossierFormBodyRequest insertDossierFormBodyRequest)
    {
        ApiRequest<InsertDossierFormBodyRequest> request = new ApiRequest<InsertDossierFormBodyRequest>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/dossier/Insert",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = insertDossierFormBodyRequest,
            IsForm = true
        };
        return await _httpClientService.SendAsync<InsertDossierFormBodyRequest, Aglou10001Response<object?>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<LoadDossierResponse>>> LoadDossierAsync(LogoutRequestBody idRequestBody)
    {
        ApiRequest<LogoutRequestBody> request = new ApiRequest<LogoutRequestBody>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/dossier/Load",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = idRequestBody
        };
        var response = await _httpClientService.SendAsync<LogoutRequestBody, Aglou10001Response<string>>(request);
        if (!response.IsSuccess && response.ErrorMessage != null)
        {
            return ApiResponse<Aglou10001Response<LoadDossierResponse>>.Error(response.ErrorMessage, HttpStatusCode.InternalServerError);
        }
        var apiResponse = ApiResponse<string>.Success(response.Data.Data, response.StatusCode);
        var transformmedResponse = GetLoadDossierResponseAsync(apiResponse);
        Aglou10001Response<LoadDossierResponse> finalResponse = new Aglou10001Response<LoadDossierResponse>
        {
            ResponseCode = (int)response.StatusCode,
            Message = response.Data.Message,
            Data = transformmedResponse,
            NbRows = response.Data.NbRows,
            TotalRows = response.Data.TotalRows
        };
        return ApiResponse<Aglou10001Response<LoadDossierResponse>>.Success(finalResponse, HttpStatusCode.OK);
    }
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<DossierSearchResponse>>>> SearchDossier(SearchDossierRequestBody searchDossierRequestBody)
    {
        ApiRequest<SearchDossierRequestBody> request = new ApiRequest<SearchDossierRequestBody>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/dossier/Search",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = searchDossierRequestBody
        };
        return await _httpClientService.SendAsync<SearchDossierRequestBody, Aglou10001Response<IEnumerable<DossierSearchResponse>>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<IEnumerable<HistroySearchResponse>>>> SearchHistroyAsync(HistorySearchRequestBody historySearchRequestBody)
    {
        ApiRequest<HistorySearchRequestBody> request = new ApiRequest<HistorySearchRequestBody>
        {
            ApiName = monopp_extern,
            Endpoint = "/api/v2/historique/Search",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = historySearchRequestBody
        };
        return await _httpClientService.SendAsync<HistorySearchRequestBody, Aglou10001Response<IEnumerable<HistroySearchResponse>>>(request);
    }
    public async Task<ApiResponse<Aglou10001Response<object>>> UpdateDossierAsync(UpdateDossierRequestBody updateDossierRequestBody)
    {
        ApiRequest<UpdateDossierRequestBody> request = new ApiRequest<UpdateDossierRequestBody>
        {
            ApiName = monopp_extern
    ,
            Endpoint = "/api/v2/dossier/Update",
            Method = HttpMethod.Post,
            RequiresApiKey = true,
            RequiresBearerToken = true,
            Data = updateDossierRequestBody
        };
        return await _httpClientService.SendAsync<UpdateDossierRequestBody, Aglou10001Response<object>>(request);
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
    #endregion
}
