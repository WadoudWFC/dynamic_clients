using MultipleHtppClient.Infrastructure;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Comment_Management.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Comment_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Demand_Management.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Demand_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Statistics_and_Counts.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.User_Account.Requests;
using MultipleHtppClient.Infrastructure.HTTP.REST;

namespace MultipleHttpClient.Application.Interfaces.Dossier;

public interface IHttpDosserAglouService
{
    Task<ApiResponse<Aglou10001Response<LoadDossierResponse>>> LoadDossierAsync(LogoutRequestBody idRequestBody);
    Task<ApiResponse<Aglou10001Response<IEnumerable<HistroySearchResponse>>>> SearchHistroyAsync(HistorySearchRequestBody historySearchRequestBody);
    Task<ApiResponse<Aglou10001Response<object>>> UpdateDossierAsync(UpdateDossierRequestBody updateDossierRequestBody);
    Task<ApiResponse<Aglou10001Response<object?>>> InsertDossierFormAsync(InsertDossierFormBodyRequest insertDossierFormBodyRequest);
    Task<ApiResponse<Aglou10001Response<object?>>> InsertCommentAsync(InsertCommentRequestBody insertCommentRequestBody);
    Task<ApiResponse<Aglou10001Response<IEnumerable<GetAllCommentsResponse>>>> GetAllCommentsAsync(GetAllCommentRequestBody getAllCommentRequestBody);
    Task<ApiResponse<Aglou10001Response<DossierCounts>>> GetCountsAsync(string userId, string roleId);
    Task<ApiResponse<Aglou10001Response<IEnumerable<DossierStatus>>>> GetDossierStatusAsync(ProfileRoleRequestBody? profileRequestBody);
    Task<ApiResponse<Aglou10001Response<IEnumerable<DossierAll>>>> GetAllDossierAsync(ProfileRoleRequestBody? profileRoleRequestBody);
    Task<ApiResponse<Aglou10001Response<IEnumerable<DossierSearchResponse>>>> SearchDossier(SearchDossierRequestBody searchDossierRequestBody);

}
