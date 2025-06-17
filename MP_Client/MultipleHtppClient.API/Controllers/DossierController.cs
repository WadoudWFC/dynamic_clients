using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultipleHttpClient.Application;
using MultipleHttpClient.Application.Dossier.Command;
using MultipleHttpClient.Application.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHtppClient.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DossierController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DossierController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("Comments")]
        public async Task<ActionResult<Result<IEnumerable<CommentSanitized>>>> GetllComments([FromBody] GetAllCommentsQuery query) => Ok(await _mediator.Send(query));
        [HttpPost("Dossiers")]
        public async Task<ActionResult<Result<IEnumerable<DossierAllSanitized>>>> GetAllDossiers([FromBody] GetAllDossierQuery query) => Ok(await _mediator.Send(query));
        [HttpPost("Counts")]
        public async Task<ActionResult<Result<DossierCountsSanitized>>> GetCounts([FromBody] GetCountsQuery query) => Ok(await _mediator.Send(query));
        [HttpPost("DossierStatus")]
        public async Task<ActionResult<Result<IEnumerable<DossierStatusSanitized>>>> GetAllDossiersStatus([FromBody] GetDossierStatusQuery query) => Ok(await _mediator.Send(query));
        [HttpPost("Comments/add")]
        public async Task<ActionResult<Result<CommentOperationResult>>> AddComment([FromBody] InsertCommentCommand command) => Ok(await _mediator.Send(command));
        [HttpPost("Dossiers/add")]
        public async Task<ActionResult<Result<InsertDossierOperationResult>>> AddDossier([FromBody] InsertDossierCommand command) => Ok(await _mediator.Send(command));
        [HttpPost("Dossiers/read")]
        public async Task<ActionResult<Result<LoadDossierResponseSanitized>>> ReadDossier([FromBody] LoadDossierQuery query) => Ok(await _mediator.Send(query));
        [HttpPost("Dossiers/search")]
        public async Task<ActionResult<Result<IEnumerable<DossierSearchSanitized>>>> SearchDossier([FromBody] SearchDossierQuery query) => Ok(await _mediator.Send(query));
        [HttpPost("History/search")]
        public async Task<ActionResult<Result<IEnumerable<HistorySearchSanitized>>>> SearchHistory([FromBody] SearchHistoryQuery query) => Ok(await _mediator.Send(query));
        [HttpPost("Dossiers/update")]
        public async Task<ActionResult<Result<DossierUpdateResult>>> UpdateDossier([FromBody] UpdateDossierCommand command) => Ok(await _mediator.Send(command));
    }
}
