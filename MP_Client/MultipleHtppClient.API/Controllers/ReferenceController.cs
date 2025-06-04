using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultipleHttpClient.Application;
using MutipleHttpClient.Domain;

namespace MultipleHtppClient.API;

[ApiController]
[Route("api/[controller]")]
public class ReferenceController : ControllerBase
{
    private readonly IMediator _mediator;
    public ReferenceController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("Activities")]
    public async Task<ActionResult<Result<IEnumerable<ActivityNatureSanitized>>>> GetAllActivities([FromBody] GetAllActivitiesQuery query) => Ok(await _mediator.Send(query));
    [HttpPost("Packs")]
    public async Task<ActionResult<Result<IEnumerable<PackSanitized>>>> GetAllPacks([FromBody] GetAllPackQuery query) => Ok(await _mediator.Send(query));
    [HttpPost("Cities")]
    public async Task<ActionResult<Result<IEnumerable<CitiesSanitized>>>> GetAllCities([FromBody] GetAllCitiesQuery query) => Ok(await _mediator.Send(query));
    [HttpPost("Arrondissements")]
    public async Task<ActionResult<Result<IEnumerable<ArrondissementSanitized>>>> GetAllArrondissements([FromBody] GetArrondissementQuery query) => Ok(await _mediator.Send(query));
    [HttpPost("TypeBien")]
    public async Task<ActionResult<Result<IEnumerable<TypeBienSanitized>>>> GetAllTypeBien([FromBody] GetTypeBienQuery query) => Ok(await _mediator.Send(query));
    [HttpPost("Regions")]
    public async Task<ActionResult<Result<IEnumerable<RegionsSanitized>>>> GetAllRegions([FromBody] GetAllRegionQuery query) => Ok(await _mediator.Send(query));
    [HttpPost("PartnersType")]
    public async Task<ActionResult<Result<IEnumerable<PartnerTypeSanitized>>>> GetAllPartnerTypes([FromBody] GetPartnerTypesQuery query) => Ok(await _mediator.Send(query));
    [HttpPost("DemandsType")]
    public async Task<ActionResult<Result<IEnumerable<DemandTypeSanitized>>>> GetAllDemandTypes([FromBody] GetDemandTypesQuery query) => Ok(await _mediator.Send(query));
    [HttpPost("CommercialCutting")]
    public async Task<ActionResult<Result<IEnumerable<CommercialCuttingSanitized>>>> GetAllCommercialCuttings([FromBody] GetCommercialCuttingQuery query) => Ok(await _mediator.Send(query));
}
