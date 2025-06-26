using MultipleHtppClient.Infrastructure.HTTP.Interfaces;
using MultipleHtppClient.Infrastructure.HTTP.REST;
using MultipleHttpClient.Application.Interfaces.Admin;

namespace MultipleHttpClient.Application.Services.Admin
{
    public class HttpAdminService : IHttpAdminService
    {
        const string monopp_extern = "aglou-q-monopp-extern";
        private readonly IHttpClientService _httpClientService;

        public HttpAdminService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        #region User Management

        public async Task<ApiResponse<object>> GetAllUsersAsync(object searchRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/utilisateur/getall",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = searchRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> SearchUsersAsync(object searchRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/utilisateur/search",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = searchRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> LoadUserAsync(object userIdRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/utilisateur/load",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = userIdRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> InsertUserAsync(object userRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/utilisateur/insert",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = userRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> UpdateUserAsync(object userRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/utilisateur/update",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = userRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> DeleteUserAsync(object userIdRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/utilisateur/delete",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = userIdRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> ValidateUserAsync(object userIdRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/utilisateur/validateuser",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = userIdRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> ResendEmailValidationAsync(object userIdRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/utilisateur/resendemailvalidateuser",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = userIdRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        #endregion

        #region Document Management

        public async Task<ApiResponse<object>> InsertDocumentAsync(object documentRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/document/insert",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = documentRequest,
                IsForm = true // Likely file upload
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> GetFilesAsync(object dossierIdRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/document/getfiles",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = dossierIdRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> GetFileAsync(object fileRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/document/getfile",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = fileRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> UpdateDocumentAsync(object documentRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/document/update",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = documentRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> DeleteDocumentAsync(object documentRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/document/delete",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = documentRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        #endregion

        #region Advanced Dossier Operations

        public async Task<ApiResponse<object>> DeleteDossierAsync(object dossierRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/dossier/delete",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = dossierRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> ChangeStatusAsync(object statusRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/dossier/changestatut",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = statusRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        #endregion

        #region Parameter Management

        public async Task<ApiResponse<object>> GetZonesAsync(object searchRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/zone",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = searchRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> GetTypePersonneAsync(object searchRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/typepersonne",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = searchRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> GetDroitProfilAsync(object searchRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/droitprofil",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = searchRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> GetDroitAsync(object searchRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/droit",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = searchRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> GetAffectationZoneUserAsync(object searchRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/AffectationZoneUser",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = searchRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> GetPartenaireAsync(object searchRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/partenaire",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = searchRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> GetLocalDossierAsync(object searchRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/localdossier",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = searchRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        public async Task<ApiResponse<object>> GetHistoriqueAsync(object searchRequest)
        {
            var request = new ApiRequest<object>
            {
                ApiName = monopp_extern,
                Endpoint = "/api/v2/historique",
                Method = HttpMethod.Post,
                RequiresApiKey = true,
                RequiresBearerToken = true,
                Data = searchRequest
            };
            return await _httpClientService.SendAsync<object, object>(request);
        }

        #endregion
    }
}
