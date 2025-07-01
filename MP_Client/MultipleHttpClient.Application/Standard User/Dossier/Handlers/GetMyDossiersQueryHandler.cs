using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Requests;
using MultipleHttpClient.Application.Dossier.Queries;
using MultipleHttpClient.Application.Interfaces.Dossier;
using MultipleHttpClient.Application.Standard_User.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application;

public class GetMyDossiersQueryHandler : IRequestHandler<GetMyDossiersQuery, Result<MyDossiersResponse>>
{
    private readonly IDossierAglouService _dossierService;
    private readonly IIdMappingService _idMappingService;
    private readonly IReferenceDataMappingService _referenceDataMappingService;
    private readonly IHttpDossierAglouService _httpDossierService;
    private readonly ILogger<GetMyDossiersQueryHandler> _logger;

    public GetMyDossiersQueryHandler(
        IDossierAglouService dossierService,
        IIdMappingService idMappingService,
        IReferenceDataMappingService referenceDataMappingService,
        IHttpDossierAglouService httpDossierService,
        ILogger<GetMyDossiersQueryHandler> logger)
    {
        _dossierService = dossierService;
        _idMappingService = idMappingService;
        _referenceDataMappingService = referenceDataMappingService;
        _httpDossierService = httpDossierService;
        _logger = logger;
    }

    public async Task<Result<MyDossiersResponse>> Handle(GetMyDossiersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting dossiers for user {UserId}, profile {RoleId}", request.UserId, request.RoleId);

            // Get internal user ID
            var internalUserId = _idMappingService.GetUserIdForGuid(request.UserId);
            if (internalUserId == null)
            {
                _logger.LogError("User ID mapping not found for {UserId}", request.UserId);
                return Result<MyDossiersResponse>.Failure(
                    new Error(Constants.DossierFail, "User not found"));
            }

            _logger.LogInformation("Internal user ID: {InternalUserId}", internalUserId.Value);

            // Create search request - bypass SearchDossierQuery validation
            var searchRequest = new SearchDossierRequestBody
            {
                Id = internalUserId.Value.ToString(),
                RoleId = request.RoleId,
                ApplyFilter = true, // Critical for role-based filtering
                TakeNumber = request.Take,
                SkipNumber = request.Skip,
                Field = "DateCreated", // Use a valid field
                Order = "desc"
            };

            _logger.LogInformation("Calling legacy API with UserId={UserId}, RoleId={RoleId}, ApplyFilter={ApplyFilter}",
                searchRequest.Id, searchRequest.RoleId, searchRequest.ApplyFilter);

            // Call legacy API directly
            var response = await _httpDossierService.SearchDossier(searchRequest);

            if (!response.IsSuccess || response.Data?.Data == null)
            {
                _logger.LogError("Legacy API error: {StatusCode}, {ErrorMessage}",
                    response.StatusCode, response.ErrorMessage);
                return Result<MyDossiersResponse>.Failure(
                    new Error(Constants.DossierFail, response.ErrorMessage ?? "Failed to retrieve dossiers"));
            }

            var dossierData = response.Data.Data.ToList();
            _logger.LogInformation("Legacy API returned {Count} dossiers", dossierData.Count);

            if (!dossierData.Any())
            {
                var emptyResponse = new MyDossiersResponse(
                    Dossiers: new List<DossierSearchSanitized>(),
                    Total: 0,
                    Take: request.Take,
                    Skip: request.Skip,
                    HasMore: false,
                    UserId: request.UserId,
                    ProfileType: GetProfileTypeName(request.RoleId)
                );
                return Result<MyDossiersResponse>.Success(emptyResponse);
            }

            // Pre-fetch mappings for performance
            var dossierIds = dossierData.Select(d => d.Id).Distinct();
            var dossierMappings = dossierIds.ToDictionary(
                id => id,
                id => _referenceDataMappingService.GetOrCreateGuidForReferenceId(id, Constants.Dossier));

            // Map to sanitized format
            var sanitizedDossiers = dossierData.Select(d =>
            {
                var localDossier = d.LocalDossier != null ? new SearchLocalDossierSanitied(
                    DossierId: dossierMappings[d.LocalDossier.Id_Dossier],
                    Id: _referenceDataMappingService.GetOrCreateGuidForReferenceId(d.LocalDossier.Id, Constants.LocalDossier),
                    Latitude: d.LocalDossier.Latitude,
                    Longitude: d.LocalDossier.Longitude,
                    Address: d.LocalDossier.Address,
                    Zone: d.LocalDossier.Zone,
                    City: d.LocalDossier.City,
                    DecopageCMR: d.LocalDossier.DecopageCMR,
                    Region: d.LocalDossier.Region
                ) : null;

                return new DossierSearchSanitized(
                    DossierId: dossierMappings[d.Id],
                    Code: d.Code,
                    NatureActivityId: d.Id_NatureActivite.HasValue ?
                        _referenceDataMappingService.GetOrCreateGuidForReferenceId(d.Id_NatureActivite.Value, Constants.Activity) : null,
                    DemandTypeId: _referenceDataMappingService.GetOrCreateGuidForReferenceId(d.Id_TypeDemande, Constants.Demand),
                    CommercialCuttingId: _referenceDataMappingService.GetOrCreateGuidForReferenceId(d.Id_DecoupageCommercial, Constants.Decoupage),
                    DossierStatusId: _referenceDataMappingService.GetOrCreateGuidForReferenceId(d.Id_StatutDossier, Constants.DStatus),
                    CanUpdate: d.CanUpdate,
                    TypeDemande: d.TypeDemande,
                    ActivityNature: d.NatureActivite,
                    Partner: d.Partenaire,
                    LocalDossierSanitized: localDossier,
                    Status: d.Statut,
                    LabelStatus: d.LabelStatut,
                    DateCreated: !string.IsNullOrEmpty(d.DateCreated) ? DateTime.Parse(d.DateCreated) : null
                )
                {
                    InternalId = d.Id,
                    InternalNatureActivityId = d.Id_NatureActivite,
                    InternalDemandTypeId = d.Id_TypeDemande,
                    InternalCommercialCuttingId = d.Id_DecoupageCommercial,
                    InternalDossierStatusId = d.Id_StatutDossier
                };
            }).ToList();

            var result = new MyDossiersResponse(
                Dossiers: sanitizedDossiers,
                Total: sanitizedDossiers.Count,
                Take: request.Take,
                Skip: request.Skip,
                HasMore: sanitizedDossiers.Count == request.Take,
                UserId: request.UserId,
                ProfileType: GetProfileTypeName(request.RoleId)
            );

            _logger.LogInformation("Successfully retrieved {Count} dossiers for user {UserId}",
                sanitizedDossiers.Count, request.UserId);

            return Result<MyDossiersResponse>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dossiers for user {UserId}", request.UserId);
            return Result<MyDossiersResponse>.Failure(
                new Error(Constants.DossierFail, "Failed to retrieve dossiers"));
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