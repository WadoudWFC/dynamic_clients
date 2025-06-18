using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MultipleHtppClient.Infrastructure;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Comment_Management.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Comment_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Commercial_Activities.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Demand_Management.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Demand_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Gepgraphical_Information.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Partner_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Property_Information.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Statistics_and_Counts.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.User_Account.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Models.User_Account.Responses;
using MultipleHtppClient.Infrastructure.HTTP.REST;
using MultipleHttpClient.Application;
using MultipleHttpClient.Application.Interfaces.User;

namespace MultipleHtppClient.API;

[ApiController]
[Route("api/[controller]")]
public class PresentationController : ControllerBase
{
    private readonly IUseHttpService _useHttpService;
    private readonly IHttpManagementAglou _httpAglouManagement;
    private readonly IHttpUserAglou _httpAglouUser;
    public PresentationController(IUseHttpService useHttpService, IHttpManagementAglou httpAglouManagement, IHttpUserAglou httpAglouUser)
    {
        _useHttpService = useHttpService;
        _httpAglouManagement = httpAglouManagement;
        _httpAglouUser = httpAglouUser;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _useHttpService.GetAllProductsAsync();

        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }

        return Ok(response.Data);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var response = await _useHttpService.GetProductByIdAsync(id);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpGet("ticker")]
    public async Task<IActionResult> Ticker()
    {
        var response = await _useHttpService.TickerAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/utilisateur/Cantrylogin")]
    public async Task<ActionResult<Aglou10001Response<AglouUser>>> CanLogin([FromBody] CanTryLoginRequestBody email)
    {
        var response = await _httpAglouUser.CanTryLoginAsync(email);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/utilisateur/Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestBody loginRequest)
    {
        var response = await _httpAglouUser.LoginAsync(loginRequest);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/dossier/getCounts")]
    public async Task<ActionResult<Aglou10001Response<DossierCounts>>> GetCounts([FromBody] GetDossierCountRequestBody requestBody)
    {
        var response = await _httpAglouManagement.GetCountsAsync(requestBody.userId, requestBody.idRole);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/typepartenaire/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<PartnersType>>>> GetAllPartnerTypes()
    {
        var response = await _httpAglouManagement.GetPartnerTypes();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/statutdossier/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<DossierStatus>>>> GetAllStatusDossier([FromBody] ProfileRoleRequestBody request)
    {
        var response = await _httpAglouManagement.GetDossierStatusAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/typedemende/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<DemandType>>>> GetAllDemandTypes()
    {
        var response = await _httpAglouManagement.GetDemandsTypeAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/decoupagecommercial/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<CommercialCutting>>>> GetCommercialCutting()
    {
        var response = await _httpAglouManagement.GetCommercialCuttingAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/dossier/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<DossierAll>>>> GetAllDossier([FromBody] ProfileRoleRequestBody request)
    {
        var response = await _httpAglouManagement.GetAllDossierAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/partenaire/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<Partner>>>> GetPartners()
    {
        var response = await _httpAglouManagement.GetPartnersAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/dossier/Search")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<DossierSearchResponse>>>> SearchDossiers([FromBody] SearchDossierRequestBody request)
    {
        var response = await _httpAglouManagement.SearchDossier(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/utilisateur/Logout")]
    public async Task<ActionResult<Aglou10001Response<object>>> Logout([FromBody] LogoutRequestBody request)
    {
        var response = await _httpAglouUser.LogoutAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/natureactivite/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<ActivityNatureResponse>>>> GetAllActivities()
    {
        var response = await _httpAglouManagement.GetAllActivitiesAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/pack/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<PackResponse>>>> GetAllPacks()
    {
        var response = await _httpAglouManagement.GetAllPacksAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/ville/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<VilleResponse>>>> GetAllCities()
    {
        var response = await _httpAglouManagement.GetAllCitiesAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/arrondissement/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<ArrondissementResponse>>>> GetArrondissements()
    {
        var response = await _httpAglouManagement.GetArrondissementsAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/typebien/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<TypeBienResponse>>>> GetAllTypeBien()
    {
        var response = await _httpAglouManagement.GetTypeBienAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/region/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<RegionResponse>>>> GetAllRegions()
    {
        var response = await _httpAglouManagement.GetAllRegionsAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/utilisateur/PasswordForgotten")]
    public async Task<ActionResult<Aglou10001Response<object>>> ForgetPassword([FromBody] ForgetPasswordRequestBody request)
    {
        var response = await _httpAglouUser.ForgetPasswordAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/utilisateur/UpdatePassWord")]
    public async Task<ActionResult<Aglou10001Response<string>>> UpdatePassword([FromBody] UpdatePasswordRequestBody request)
    {
        var response = await _httpAglouUser.UpdatePasswordAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/dossier/Load")]
    public async Task<ActionResult<Aglou10001Response<LoadDossierResponse>>> LoadDossier([FromBody] LogoutRequestBody request)
    {
        var response = await _httpAglouManagement.LoadDossierAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        var apiReq = ApiResponse<string>.Success(response.Data.Data, response.StatusCode);
        var transformmedResponse = GetLoadDossierResponseAsync(apiReq);
        Aglou10001Response<LoadDossierResponse> finalResponse = new Aglou10001Response<LoadDossierResponse>
        {
            ResponseCode = (int)response.StatusCode,
            Message = response.Data.Message,
            Data = transformmedResponse,
            NbRows = response.Data.NbRows,
            TotalRows = response.Data.TotalRows
        };
        return Ok(finalResponse);
    }
    [HttpPost("/api/v2/historique/Search")]
    public async Task<ActionResult<Aglou10001Response<HistroySearchResponse>>> SearchHistory([FromBody] HistorySearchRequestBody request)
    {
        var response = await _httpAglouManagement.SearchHistroyAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/dossier/Update")]
    public async Task<ActionResult<Aglou10001Response<object>>> UpdateDossier([FromBody] UpdateDossierRequestBody request)
    {
        var response = await _httpAglouManagement.UpdateDossierAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/dossier/Insert")]
    public async Task<ActionResult<Aglou10001Response<object>>> InsertDossier([FromForm] InsertDossierFormBodyRequest request)
    {
        var response = await _httpAglouManagement.InsertDossierFormAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/commentaire/Insert")]
    public async Task<ActionResult<Aglou10001Response<object>>> InsertComment([FromBody] InsertCommentRequestBody request)
    {
        var response = await _httpAglouManagement.InsertCommentAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/commentaire/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<GetAllCommentsResponse>>>> GetAllComments([FromBody] GetAllCommentRequestBody request)
    {
        var response = await _httpAglouManagement.GetAllCommentsAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/utilisateur/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<GetAllUsersResponse>>>> GetAllUsers([FromBody] GetAllUsersRequestBody request)
    {
        var response = await _httpAglouManagement.GetAllUsersAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }

    // Important !!: Refactor code to reduce arbitrary method defintion
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

}
