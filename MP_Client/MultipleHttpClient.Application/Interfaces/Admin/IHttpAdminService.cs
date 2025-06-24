using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultipleHtppClient.Infrastructure.HTTP.REST;

namespace MultipleHttpClient.Application.Interfaces.Admin
{
    public interface IHttpAdminService
    {
        // User Management
        Task<ApiResponse<object>> GetAllUsersAsync(object searchRequest);
        Task<ApiResponse<object>> SearchUsersAsync(object searchRequest);
        Task<ApiResponse<object>> LoadUserAsync(object userIdRequest);
        Task<ApiResponse<object>> InsertUserAsync(object userRequest);
        Task<ApiResponse<object>> UpdateUserAsync(object userRequest);
        Task<ApiResponse<object>> DeleteUserAsync(object userIdRequest);
        Task<ApiResponse<object>> ValidateUserAsync(object userIdRequest);
        Task<ApiResponse<object>> ResendEmailValidationAsync(object userIdRequest);

        // Document Management
        Task<ApiResponse<object>> InsertDocumentAsync(object documentRequest);
        Task<ApiResponse<object>> GetFilesAsync(object dossierIdRequest);
        Task<ApiResponse<object>> GetFileAsync(object fileRequest);
        Task<ApiResponse<object>> UpdateDocumentAsync(object documentRequest);
        Task<ApiResponse<object>> DeleteDocumentAsync(object documentRequest);

        // Advanced Dossier Operations
        Task<ApiResponse<object>> DeleteDossierAsync(object dossierRequest);
        Task<ApiResponse<object>> ChangeStatusAsync(object statusRequest);

        // Parameter Management
        Task<ApiResponse<object>> GetZonesAsync(object searchRequest);
        Task<ApiResponse<object>> GetTypePersonneAsync(object searchRequest);
        Task<ApiResponse<object>> GetDroitProfilAsync(object searchRequest);
        Task<ApiResponse<object>> GetDroitAsync(object searchRequest);
        Task<ApiResponse<object>> GetAffectationZoneUserAsync(object searchRequest);
        Task<ApiResponse<object>> GetPartenaireAsync(object searchRequest);
        Task<ApiResponse<object>> GetLocalDossierAsync(object searchRequest);
        Task<ApiResponse<object>> GetHistoriqueAsync(object searchRequest);
    }
}
