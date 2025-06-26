using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Requests;
using MultipleHttpClient.Application.Interfaces.Dossier;
using MultipleHttpClient.Application.Standard_User.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Standard_User.Dossier.Handlers
{
    public class GetMyDossiersSimpleHandler : IRequestHandler<GetMyDossiersQuery, Result<IEnumerable<DossierSearchSanitized>>>
    {
        private readonly IHttpDossierAglouService _httpDossierService;
        private readonly IDossierAglouService _dossierService;
        private readonly IIdMappingService _idMappingService;
        private readonly IReferenceDataMappingService _referenceDataMappingService;
        private readonly ILogger<GetMyDossiersSimpleHandler> _logger;

        public GetMyDossiersSimpleHandler(
            IHttpDossierAglouService httpDossierService,
            IDossierAglouService dossierService,
            IIdMappingService idMappingService,
            IReferenceDataMappingService referenceDataMappingService,
            ILogger<GetMyDossiersSimpleHandler> logger)
        {
            _httpDossierService = httpDossierService;
            _dossierService = dossierService;
            _idMappingService = idMappingService;
            _referenceDataMappingService = referenceDataMappingService;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<DossierSearchSanitized>>> Handle(
            GetMyDossiersQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting dossiers for user {UserId} with role {RoleId}",
                    request.UserId, request.RoleId);

                // Get internal user ID
                var internalUserId = _idMappingService.GetUserIdForGuid(request.UserId);
                if (internalUserId == null)
                {
                    _logger.LogError("User ID mapping not found for {UserId}", request.UserId);
                    return Result<IEnumerable<DossierSearchSanitized>>.Failure(
                        new Error(Constants.DossierFail, "User not found"));
                }

                // Call the HTTP service directly with the proper request format
                var searchRequest = new SearchDossierRequestBody
                {
                    Id = internalUserId.Value.ToString(), // This is the key - internal user ID as string
                    RoleId = request.RoleId,
                    ApplyFilter = true,
                    Code = null,
                    DosseriStatusId = null,
                    DemandTypeId = null,
                    NatureOfActivityId = null,
                    PartnerId = null,
                    CommercialCuttingId = null,
                    StartDate = null,
                    EndDate = null,
                    TakeNumber = request.Take ?? 50,
                    SkipNumber = request.Skip ?? 0,
                    Field = request.Field ?? "date_created",
                    Order = request.Order ?? "desc"
                };

                var response = await _httpDossierService.SearchDossier(searchRequest);

                if (!response.IsSuccess || response.Data?.Data == null)
                {
                    _logger.LogError("Failed to retrieve dossiers from legacy API");
                    return Result<IEnumerable<DossierSearchSanitized>>.Failure(
                        new Error(Constants.DossierFail, "Failed to retrieve dossiers"));
                }

                // Map the response using the existing DossierAglouService logic
                var dossierData = response.Data.Data.ToList();

                // Pre-fetch all necessary mappings for better performance
                var dossierIds = dossierData.Select(d => d.Id).Distinct();
                var dossierMappings = dossierIds.ToDictionary(
                    id => id,
                    id => _referenceDataMappingService.GetOrCreateGuidForReferenceId(id, Constants.Dossier));

                var natureActivityIds = dossierData
                    .Where(d => d.Id_NatureActivite.HasValue)
                    .Select(d => d.Id_NatureActivite.Value)
                    .Distinct();
                var natureActivityMappings = natureActivityIds.ToDictionary(
                    id => id,
                    id => _referenceDataMappingService.GetOrCreateGuidForReferenceId(id, Constants.Activity));

                var demandTypeIds = dossierData.Select(d => d.Id_TypeDemande).Distinct();
                var demandTypeMappings = demandTypeIds.ToDictionary(
                    id => id,
                    id => _referenceDataMappingService.GetOrCreateGuidForReferenceId(id, Constants.Demand));

                var commercialCuttingIds = dossierData.Select(d => d.Id_DecoupageCommercial).Distinct();
                var commercialCuttingMappings = commercialCuttingIds.ToDictionary(
                    id => id,
                    id => _referenceDataMappingService.GetOrCreateGuidForReferenceId(id, Constants.Decoupage));

                var dossierStatusIds = dossierData.Select(d => d.Id_StatutDossier).Distinct();
                var dossierStatusMappings = dossierStatusIds.ToDictionary(
                    id => id,
                    id => _referenceDataMappingService.GetOrCreateGuidForReferenceId(id, Constants.DStatus));

                // Map and sanitize the response
                var results = dossierData.Select(d =>
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
                        NatureActivityId: d.Id_NatureActivite.HasValue ? natureActivityMappings[d.Id_NatureActivite.Value] : null,
                        DemandTypeId: demandTypeMappings[d.Id_TypeDemande],
                        CommercialCuttingId: commercialCuttingMappings[d.Id_DecoupageCommercial],
                        DossierStatusId: dossierStatusMappings[d.Id_StatutDossier],
                        CanUpdate: d.CanUpdate,
                        TypeDemande: d.TypeDemande,
                        ActivityNature: d.NatureActivite,
                        Partner: d.Partenaire,
                        LocalDossierSanitized: localDossier,
                        Status: d.Statut,
                        LabelStatus: d.LabelStatut,
                        DateCreated: !string.IsNullOrEmpty(d.DateCreated) ? DateTime.Parse(d.DateCreated) : null
                    );
                }).ToList();

                _logger.LogInformation("Successfully retrieved {Count} dossiers for user {UserId}",
                    results.Count, request.UserId);

                return Result<IEnumerable<DossierSearchSanitized>>.Success(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dossiers for user {UserId}", request.UserId);
                return Result<IEnumerable<DossierSearchSanitized>>.Failure(
                    new Error(Constants.DossierFail, "Failed to retrieve dossiers"));
            }
        }
    }
}
