using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MultipleHtppClient.Infrastructure;
using MultipleHtppClient.Infrastructure.Models.Gestion.Requests;
using MultipleHtppClient.Infrastructure.Models.Gestion.Responses;

namespace MultipleHtppClient.API;

[ApiController]
[Route("api/[controller]")]
public class PresentationController : ControllerBase
{
    private readonly IUseHttpService _useHttpService;

    public PresentationController(IUseHttpService useHttpService)
    {
        _useHttpService = useHttpService;
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
        var response = await _useHttpService.CanTryLoginAsync(email);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/utilisateur/Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestBody loginRequest)
    {
        var response = await _useHttpService.LoginAsync(loginRequest);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/dossier/getCounts")]
    public async Task<ActionResult<Aglou10001Response<DossierCounts>>> GetCounts([FromBody] GetDossierCountRequestBody requestBody)
    {
        var response = await _useHttpService.GetCountsAsync(requestBody.userId, requestBody.idRole);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/typepartenaire/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<PartnersType>>>> GetAllPartnerTypes()
    {
        var response = await _useHttpService.GetPartnerTypes();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/statutdossier/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<DossierStatus>>>> GetAllStatusDossier([FromBody] ProfileRoleRequestBody request)
    {
        var response = await _useHttpService.GetDossierStatusAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/typedemende/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<DemandType>>>> GetAllDemandTypes()
    {
        var response = await _useHttpService.GetDemandsTypeAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/decoupagecommercial/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<CommercialCutting>>>> GetCommercialCutting()
    {
        var response = await _useHttpService.GetCommercialCuttingAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/dossier/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<DossierAll>>>> GetAllDossier([FromBody] ProfileRoleRequestBody request)
    {
        var response = await _useHttpService.GetAllDossierAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/partenaire/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<Partner>>>> GetPartners()
    {
        var response = await _useHttpService.GetPartnersAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/dossier/Search")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<DossierSearchResponse>>>> SearchDossiers([FromBody] SearchDossierRequestBody request)
    {
        var response = await _useHttpService.SearchDossier(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/utilisateur/Logout")]
    public async Task<ActionResult<Aglou10001Response<object>>> Logout([FromBody] LogoutRequestBody request)
    {
        var response = await _useHttpService.LogoutAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/natureactivite/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<ActivityNatureResponse>>>> GetAllActivities()
    {
        var response = await _useHttpService.GetAllActivitiesAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/pack/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<PackResponse>>>> GetAllPacks()
    {
        var response = await _useHttpService.GetAllPacksAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/ville/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<VilleResponse>>>> GetAllCities()
    {
        var response = await _useHttpService.GetAllCitiesAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/arrondissement/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<ArrondissementResponse>>>> GetArrondissements()
    {
        var response = await _useHttpService.GetArrondissementsAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/typebien/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<TypeBienResponse>>>> GetAllTypeBien()
    {
        var response = await _useHttpService.GetTypeBienAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/region/GetAll")]
    public async Task<ActionResult<Aglou10001Response<IEnumerable<RegionResponse>>>> GetAllRegions()
    {
        var response = await _useHttpService.GetAllRegionsAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/utilisateur/PasswordForgotten")]
    public async Task<ActionResult<Aglou10001Response<object>>> ForgetPassword([FromBody] ForgetPasswordRequestBody request)
    {
        var response = await _useHttpService.ForgetPasswordAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/utilisateur/UpdatePassWord")]
    public async Task<ActionResult<Aglou10001Response<string>>> UpdatePassword([FromBody] UpdatePasswordRequestBody request)
    {
        var response = await _useHttpService.UpdatePasswordAsync(request);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("/api/v2/dossier/Load")]
    public async Task<ActionResult<Aglou10001Response<LoadDossierResponse>>> LoadDossier([FromBody] LogoutRequestBody request)
    {
        var response = await _useHttpService.LoadDossierAsync(request);
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
