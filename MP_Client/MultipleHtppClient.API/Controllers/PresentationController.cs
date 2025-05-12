using System.Security.Cryptography.X509Certificates;
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
}
