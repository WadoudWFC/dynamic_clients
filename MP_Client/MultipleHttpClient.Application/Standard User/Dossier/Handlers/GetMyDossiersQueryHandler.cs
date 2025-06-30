using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Queries;
using MultipleHttpClient.Application.Standard_User.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application;

public class GetMyDossiersQueryHandler : IRequestHandler<GetMyDossiersQuery, Result<IEnumerable<DossierSearchSanitized>>>
{
    private readonly IDossierAglouService _dossierService;
    private readonly IIdMappingService _idMappingService;
    private readonly ILogger<GetMyDossiersQueryHandler> _logger;

    public GetMyDossiersQueryHandler(
        IDossierAglouService dossierService,
        IIdMappingService idMappingService,
        ILogger<GetMyDossiersQueryHandler> logger)
    {
        _dossierService = dossierService;
        _idMappingService = idMappingService;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<DossierSearchSanitized>>> Handle(GetMyDossiersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting all dossiers for user {0} with role {1}", request.UserId, request.RoleId);

            // Get internal user ID for the search
            var internalUserId = _idMappingService.GetUserIdForGuid(request.UserId);
            if (internalUserId == null)
            {
                _logger.LogError("User ID mapping not found for {0}", request.UserId);
                return Result<IEnumerable<DossierSearchSanitized>>.Failure(new Error(Constants.DossierFail, "User not found"));
            }

            // Create search query without any filters to get all accessible dossiers
            var searchQuery = new SearchDossierQuery
            {
                UserId = request.UserId,
                RoleId = request.RoleId,
                InternalUserId = internalUserId.Value,
                ApplyFilter = true,
                Take = request.Take ?? 50,
                Skip = request.Skip ?? 0,
                Order = request.Order ?? "desc",
                Field = request.Field ?? "date_created"
            };

            // Use the existing SearchDossierAsync method which already handles all the mapping
            var result = await _dossierService.SearchDossierAsync(searchQuery);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to retrieve dossiers for user {0}: {1}", request.UserId, result.Error.Message);
                return result;
            }

            var dossiers = result.Value?.ToList() ?? new List<DossierSearchSanitized>();

            _logger.LogInformation("Successfully retrieved {0} dossiers for user {1}", dossiers.Count, request.UserId);

            return Result<IEnumerable<DossierSearchSanitized>>.Success(dossiers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dossiers for user {0}", request.UserId);
            return Result<IEnumerable<DossierSearchSanitized>>.Failure(new Error(Constants.DossierFail, "Failed to retrieve dossiers"));
        }
    }
}