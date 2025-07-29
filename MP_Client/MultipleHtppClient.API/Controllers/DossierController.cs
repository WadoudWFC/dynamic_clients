using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultipleHttpClient.Application;
using MultipleHttpClient.Application.Dossier.Command;
using MultipleHttpClient.Application.Dossier.Queries;
using MultipleHttpClient.Application.Services.Security;
using MultipleHttpClient.Application.Standard_User.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHtppClient.API.Controllers
{
    [ApiController]
    [Route("api/bff/v2/[controller]")]
    [Authorize]
    public class DossierController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DossierController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{dossierId}")]
        [RequireProfile(1, 2)]
        public async Task<IActionResult> GetDossier(Guid dossierId)
        {
            var query = new LoadDossierQuery(dossierId);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        [RequireProfile(1, 2, 3)]
        public async Task<IActionResult> CreateDossier([FromBody] InsertDossierCommand command)
        {
            var userId = GetCurrentUserId();
            var commandWithUser = command with { UserId = userId };

            var result = await _mediator.Send(commandWithUser);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("{dossierId}")]
        [OwnershipAuthorization("dossierId", "dossier")]
        public async Task<IActionResult> UpdateDossier(Guid dossierId, [FromBody] UpdateDossierCommand command)
        {
            var userId = GetCurrentUserId();
            var commandWithIds = command with { DossierId = dossierId, UserId = userId };

            var result = await _mediator.Send(commandWithIds);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost("search")]
        [RequireProfile(1, 2, 3)]
        public async Task<IActionResult> SearchDossiers([FromBody] SearchDossierQuery query)
        {
            var userId = GetCurrentUserId();
            var profileId = GetCurrentInternalProfileId();

            var queryWithUser = query with
            {
                UserId = userId,
                RoleId = profileId
            };

            var result = await _mediator.Send(queryWithUser);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet]
        [RequireProfile(1)]
        public async Task<IActionResult> GetAllDossiers([FromQuery] string? roleId)
        {
            var query = new GetAllDossierQuery
            {
                UserId = GetCurrentUserId(),
                RoleId = roleId ?? GetCurrentInternalProfileId()
            };

            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("counts")]
        [RequireProfile(1, 2, 3)]
        public async Task<IActionResult> GetCounts([FromQuery] string? roleId)
        {
            var query = new GetCountsQuery
            {
                UserId = GetCurrentUserId(),
                RoleId = roleId ?? GetCurrentInternalProfileId()
            };

            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost("my-dossiers")]
        [Authorize]
        public async Task<IActionResult> GetMyDossiers([FromBody] SearchDossierQuery searchQuery)
        {
            var userId = GetCurrentUserId();
            var profileId = GetCurrentInternalProfileId();

            if (userId == Guid.Empty)
            {
                return Unauthorized(new { error = "Invalid user context" });
            }

            var queryWithUser = searchQuery with
            {
                UserId = userId,
                RoleId = profileId
            };

            var result = await _mediator.Send(queryWithUser);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpGet("my-dossiers/ids")]
        [Authorize]
        public async Task<IActionResult> GetMyDossierIds([FromQuery] int? take, [FromQuery] int? skip)
        {
            var userId = GetCurrentUserId();
            var profileId = GetCurrentInternalProfileId();

            if (userId == Guid.Empty)
            {
                return Unauthorized(new { error = "Invalid user context" });
            }

            var query = new GetMyDossierIdsQuery
            {
                UserId = userId,
                RoleId = profileId,
                Take = take ?? 100,
                Skip = skip ?? 0
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Error.Message });
            }

            return Ok(result.Value);
        }

        [HttpGet("my-dossiers/detailed")]
        [Authorize]
        public async Task<IActionResult> GetMyDossiersDetailed()
        {
            var userId = GetCurrentUserId();
            var profileId = GetCurrentInternalProfileId();

            if (userId == Guid.Empty)
            {
                return Unauthorized(new { error = "Invalid user context" });
            }

            var query = new GetMyDossiersDetailedQuery
            {
                UserId = userId,
                RoleId = profileId
            };

            var result = await _mediator.Send(query);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(new { error = result.Error.Message });
        }

        #region Comment Management

        [HttpGet("{dossierId}/comments")]
        [OwnershipAuthorization("dossierId", "dossier")]
        public async Task<IActionResult> GetDossierComments(Guid dossierId)
        {
            var query = new GetAllCommentsQuery
            {
                DossierId = dossierId,
                UserId = GetCurrentUserId()
            };

            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost("{dossierId}/comments")]
        [OwnershipAuthorization("dossierId", "dossier")]
        public async Task<IActionResult> AddComment(Guid dossierId, [FromBody] AddCommentRequest request)
        {
            var command = new InsertCommentCommand
            {
                DossierId = dossierId,
                UserId = GetCurrentUserId(),
                Content = request.Content
            };

            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        #endregion

        #region Status Management

        [HttpGet("statuses")]
        [RequireProfile(1, 2, 3)]
        public async Task<IActionResult> GetDossierStatuses()
        {
            var query = new GetDossierStatusQuery
            {
                UserId = GetCurrentUserId()
            };

            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        #endregion

        #region History Management
        [HttpPost("{dossierId}/history")]
        [OwnershipAuthorization("dossierId", "dossier")]
        public async Task<IActionResult> SearchHistory(Guid dossierId, [FromBody] HistorySearchRequest request)
        {
            var query = new SearchHistoryQuery
            {
                DossierId = dossierId,
                Field = request.Field ?? "date_created",
                Order = request.Order ?? "desc",
                Skip = request.Skip.Value,
                Take = request.Take.Value
            };

            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        #endregion

        #region Helper Methods

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("user_id")?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        private int GetCurrentInternalUserId()
        {
            var userGuid = GetCurrentUserId();
            if (userGuid == Guid.Empty) return 0;

            var idMappingService = HttpContext.RequestServices.GetRequiredService<IIdMappingService>();
            return idMappingService.GetUserIdForGuid(userGuid) ?? 0;
        }

        private string GetCurrentInternalProfileId()
        {
            var profileGuidClaim = User.FindFirst("profile_id")?.Value;
            if (string.IsNullOrEmpty(profileGuidClaim) || !Guid.TryParse(profileGuidClaim, out var profileGuid))
                return "3";

            var referenceDataMappingService = HttpContext.RequestServices.GetRequiredService<IReferenceDataMappingService>();
            var profileId = referenceDataMappingService.GetReferenceIdForGuid(profileGuid, Constants.Profile);
            return profileId?.ToString() ?? "3";
        }

        private int? GetCurrentCommercialDivisionId()
        {
            var commercialDivisionGuidClaim = User.FindFirst("commercial_division_id")?.Value;
            if (string.IsNullOrEmpty(commercialDivisionGuidClaim) || !Guid.TryParse(commercialDivisionGuidClaim, out var commercialDivisionGuid))
                return null;

            var referenceDataMappingService = HttpContext.RequestServices.GetRequiredService<IReferenceDataMappingService>();
            return referenceDataMappingService.GetReferenceIdForGuid(commercialDivisionGuid, Constants.CommercialDivision);
        }
        #endregion
    }
}