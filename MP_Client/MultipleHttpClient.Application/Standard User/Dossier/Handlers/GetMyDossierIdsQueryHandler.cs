using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application;

public class GetMyDossierIdsQueryHandler : IRequestHandler<GetMyDossierIdsQuery, Result<MyDossierIdsResponse>>
{
    private readonly IDossierAglouService _dossierService;
    private readonly IIdMappingService _idMappingService;
    private readonly IReferenceDataMappingService _referenceDataMappingService;
    private readonly ILogger<GetMyDossierIdsQueryHandler> _logger;

    public GetMyDossierIdsQueryHandler(
        IDossierAglouService dossierService,
        IIdMappingService idMappingService,
        IReferenceDataMappingService referenceDataMappingService,
        ILogger<GetMyDossierIdsQueryHandler> logger)
    {
        _dossierService = dossierService;
        _idMappingService = idMappingService;
        _referenceDataMappingService = referenceDataMappingService;
        _logger = logger;
    }

    public async Task<Result<MyDossierIdsResponse>> Handle(GetMyDossierIdsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting dossier IDs for user {UserId}", request.UserId);

            // Use SearchDossierAsync with minimal data requirements
            var searchQuery = new SearchDossierQuery
            {
                UserId = request.UserId,
                RoleId = request.RoleId,
                ApplyFilter = true,
                Take = request.Take,
                Skip = request.Skip,
                Order = "desc",
                Field = "date_created"
            };

            var searchResult = await _dossierService.SearchDossierAsync(searchQuery);

            if (!searchResult.IsSuccess)
            {
                _logger.LogError("Failed to search dossiers for user {UserId}: {Error}",
                    request.UserId, searchResult.Error.Message);
                return Result<MyDossierIdsResponse>.Failure(searchResult.Error);
            }

            var dossiers = searchResult.Value?.ToList() ?? new List<DossierSearchSanitized>();

            // Map to lightweight response
            var dossierIds = dossiers.Select(d => new DossierIdInfo(
                DossierId: d.DossierId,
                Code: d.Code ?? "N/A",
                Status: d.Status ?? "Unknown",
                DateCreated: d.DateCreated
            )
            {
                InternalId = d.InternalId
            }).ToList();

            var response = new MyDossierIdsResponse(
                DossierIds: dossierIds,
                Total: dossierIds.Count,
                Take: request.Take,
                Skip: request.Skip,
                HasMore: dossierIds.Count == request.Take,
                UserId: request.UserId,
                ProfileType: GetProfileTypeName(request.RoleId)
            );

            _logger.LogInformation("Successfully retrieved {0} dossier IDs for user {1}",
                dossierIds.Count, request.UserId);

            return Result<MyDossierIdsResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dossier IDs for user {UserId}", request.UserId);
            return Result<MyDossierIdsResponse>.Failure(
                new Error(Constants.DossierFail, "Failed to retrieve dossier IDs"));
        }
    }

    private static string GetProfileTypeName(string profileId) => profileId switch
    {
        "1" => "Administrator",
        "2" => "Regional Administrator",
        "3" => "Standard User",
        _ => "Unknown"
    };
}