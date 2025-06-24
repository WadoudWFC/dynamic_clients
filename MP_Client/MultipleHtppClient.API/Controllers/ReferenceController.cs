using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultipleHttpClient.Application;
using MultipleHttpClient.Application.Services.Security;

namespace MultipleHtppClient.API;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReferenceController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReferenceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Reference data endpoints - accessible to all authenticated users
    /// No ownership validation needed as this is lookup data
    /// </summary>

    [HttpGet("activities")]
    [RequireProfile(1, 2, 3)]
    public async Task<IActionResult> GetActivities()
    {
        var query = new GetAllActivitiesQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("cities")]
    [RequireProfile(1, 2, 3)]
    public async Task<IActionResult> GetCities()
    {
        var query = new GetAllCitiesQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("regions")]
    [RequireProfile(1, 2, 3)]
    public async Task<IActionResult> GetRegions()
    {
        var query = new GetAllRegionQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("arrondissements")]
    [RequireProfile(1, 2, 3)]
    public async Task<IActionResult> GetArrondissements()
    {
        var query = new GetArrondissementQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("demand-types")]
    [RequireProfile(1, 2, 3)]
    public async Task<IActionResult> GetDemandTypes()
    {
        var query = new GetDemandTypesQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("partner-types")]
    [RequireProfile(1, 2, 3)]
    public async Task<IActionResult> GetPartnerTypes()
    {
        var query = new GetPartnerTypesQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("commercial-cuttings")]
    [RequireProfile(1, 2, 3)]
    public async Task<IActionResult> GetCommercialCuttings()
    {
        var query = new GetCommercialCuttingQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("type-bien")]
    [RequireProfile(1, 2, 3)]
    public async Task<IActionResult> GetTypeBien()
    {
        var query = new GetTypeBienQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("packs")]
    [RequireProfile(1, 2, 3)]
    public async Task<IActionResult> GetPacks()
    {
        var query = new GetAllPackQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
