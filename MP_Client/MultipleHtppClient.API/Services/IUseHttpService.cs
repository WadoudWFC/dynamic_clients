using MultipleHtppClient.Infrastructure;
using MultipleHtppClient.Infrastructure.Models.Gestion.Requests;
using MultipleHtppClient.Infrastructure.Models.Gestion.Responses;

namespace MultipleHtppClient.API;

public interface IUseHttpService
{
    Task<ApiResponse<IEnumerable<Product>>> GetAllProductsAsync();
    Task<ApiResponse<Product>> GetProductByIdAsync(int id);
    Task<ApiResponse<object>> TickerAsync();
    Task<ApiResponse<Aglou10001Response<AglouUser>>> CanTryLoginAsync(CanTryLoginRequestBody email);
    Task<ApiResponse<Aglou10001Response<AglouLoginResponse>>> LoginAsync(LoginRequestBody loginRequestBody);
    Task<ApiResponse<Aglou10001Response<DossierCounts>>> GetCountsAsync(string userId, string roleId);
    Task<ApiResponse<Aglou10001Response<IEnumerable<PartnersType>>>> GetPartnerTypes();
    Task<ApiResponse<Aglou10001Response<IEnumerable<DossierStatus>>>> GetDossierStatusAsync(ProfileRoleRequestBody? profileRequestBody);
    Task<ApiResponse<Aglou10001Response<IEnumerable<DemandType>>>> GetDemandsTypeAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<CommercialCutting>>>> GetCommercialCuttingAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<DossierAll>>>> GetAllDossierAsync(ProfileRoleRequestBody? profileRoleRequestBody);
    Task<ApiResponse<Aglou10001Response<IEnumerable<Partner>>>> GetPartnersAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<DossierSearchResponse>>>> SearchDossier(SearchDossierRequestBody searchDossierRequestBody);
    Task<ApiResponse<Aglou10001Response<object>>> LogoutAsync(LogoutRequestBody loginRequestBody);
    Task<ApiResponse<Aglou10001Response<IEnumerable<ActivityNatureResponse>>>> GetAllActivitiesAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<PackResponse>>>> GetAllPacksAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<VilleResponse>>>> GetAllCitiesAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<ArrondissementResponse>>>> GetArrondissementsAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<TypeBienResponse>>>> GetTypeBienAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<RegionResponse>>>> GetAllRegionsAsync();
    Task<ApiResponse<Aglou10001Response<object>>> ForgetPasswordAsync(ForgetPasswordRequestBody forgetPasswordRequestBody);
    Task<ApiResponse<Aglou10001Response<object>>> UpdatePasswordAsync(UpdatePasswordRequestBody updatePasswordRequestBody);
    Task<ApiResponse<Aglou10001Response<string>>> LoadDossierAsync(LogoutRequestBody idRequestBody);
    Task<ApiResponse<Aglou10001Response<IEnumerable<HistroySearchResponse>>>> SearchHistroyAsync(HistorySearchRequestBody historySearchRequestBody);
    Task<ApiResponse<Aglou10001Response<object>>> UpdateDossierAsync(UpdateDossierRequestBody updateDossierRequestBody);
    Task<ApiResponse<Aglou10001Response<object?>>> InsertDossierFormAsync(InsertDossierFormBodyRequest insertDossierFormBodyRequest);
    Task<ApiResponse<Aglou10001Response<object?>>> InsertCommentAsync(InsertCommentRequestBody insertCommentRequestBody);
    Task<ApiResponse<Aglou10001Response<IEnumerable<GetAllCommentsResponse>>>> GetAllCommentsAsync(GetAllCommentRequestBody getAllCommentRequestBody);
}

