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
}