using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Queries;
using MultipleHttpClient.Application.Standard_User.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Standard_User.Dossier.Handlers
{
    public class GetMyAllDossiersQueryHandler : IRequestHandler<GetMyDossiersQuery, Result<IEnumerable<DossierSearchSanitized>>>
    {
        private readonly IDossierAglouService _dossierService;
        private readonly IIdMappingService _idMappingService;
        private readonly IReferenceDataMappingService _referenceDataMappingService;
        private readonly ILogger<GetMyAllDossiersQueryHandler> _logger;

        public GetMyAllDossiersQueryHandler(
            IDossierAglouService dossierService,
            IIdMappingService idMappingService,
            IReferenceDataMappingService referenceDataMappingService,
            ILogger<GetMyAllDossiersQueryHandler> logger)
        {
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
                _logger.LogInformation("Getting all dossiers for user {0} with role {1}",
                    request.UserId, request.RoleId);

                // First, try to get all dossiers using GetAllDossierAsync
                var getAllQuery = new GetAllDossierQuery
                {
                    UserId = request.UserId,
                    RoleId = request.RoleId
                };

                var allDossiersResult = await _dossierService.GetAllDossierAsync(getAllQuery);

                if (allDossiersResult.IsSuccess && allDossiersResult.Value?.Any() == true)
                {
                    // Convert DossierAllSanitized to DossierSearchSanitized
                    var dossiersList = allDossiersResult.Value.ToList();
                    var searchResults = new List<DossierSearchSanitized>();

                    foreach (var dossier in dossiersList)
                    {
                        // Load full details for each dossier
                        var loadQuery = new LoadDossierQuery(dossier.Dossier);
                        var loadResult = await _dossierService.LoadDossierAsync(loadQuery);

                        if (loadResult.IsSuccess && loadResult.Value != null)
                        {
                            var details = loadResult.Value;

                            // Create DossierSearchSanitized from loaded details
                            var searchDossier = new DossierSearchSanitized(
                                DossierId: dossier.Dossier,
                                Code: dossier.Code,
                                NatureActivityId: details.ActivityNatureId,
                                DemandTypeId: dossier.DemandTypeId,
                                CommercialCuttingId: Guid.Empty,
                                DossierStatusId: dossier.DossierStatusId,
                                CanUpdate: details.CanUpload,
                                TypeDemande: details.RequestType,
                                ActivityNature: null,
                                Partner: details.Partner?.FirstName + " " + details.Partner?.LastName,
                                LocalDossierSanitized: details.LocalDossier != null ? new SearchLocalDossierSanitied(
                                    DossierId: dossier.Dossier,
                                    Id: details.LocalDossier.Id,
                                    Latitude: details.LocalDossier.Latitude,
                                    Longitude: details.LocalDossier.Longitude,
                                    Address: details.LocalDossier.Address,
                                    Zone: details.LocalDossier.Zone,
                                    City: details.LocalDossier.CityName,
                                    DecopageCMR: null,
                                    Region: details.LocalDossier.Region
                                ) : null,
                                Status: dossier.Status,
                                LabelStatus: details.StatusLabel,
                                DateCreated: null
                            );

                            searchResults.Add(searchDossier);
                        }
                        else
                        {
                            // If we can't load details, create basic entry
                            var basicDossier = new DossierSearchSanitized(
                                DossierId: dossier.Dossier,
                                Code: dossier.Code,
                                NatureActivityId: null,
                                DemandTypeId: dossier.DemandTypeId,
                                CommercialCuttingId: Guid.Empty,
                                DossierStatusId: dossier.DossierStatusId,
                                CanUpdate: false,
                                TypeDemande: null,
                                ActivityNature: null,
                                Partner: null,
                                LocalDossierSanitized: null,
                                Status: dossier.Status,
                                LabelStatus: null,
                                DateCreated: null
                            );
                            searchResults.Add(basicDossier);
                        }
                    }

                    // Apply pagination
                    var paginatedResults = searchResults
                        .Skip(request.Skip ?? 0)
                        .Take(request.Take ?? 50)
                        .ToList();

                    _logger.LogInformation("Successfully retrieved {0} dossiers for user {1}",
                        paginatedResults.Count, request.UserId);

                    return Result<IEnumerable<DossierSearchSanitized>>.Success(paginatedResults);
                }

                // If GetAllDossierAsync didn't work, fall back to search
                _logger.LogWarning("GetAllDossierAsync returned no results, trying SearchDossierAsync");

                var internalUserId = _idMappingService.GetUserIdForGuid(request.UserId);
                if (internalUserId == null)
                {
                    _logger.LogError("User ID mapping not found for {0}", request.UserId);
                    return Result<IEnumerable<DossierSearchSanitized>>.Failure(
                        new Error(Constants.DossierFail, "User not found"));
                }

                var searchQuery = new SearchDossierQuery
                {
                    UserId = request.UserId,
                    RoleId = request.RoleId,
                    InternalUserId = internalUserId.Value,
                    ApplyFilter = false, // Try without filter first
                    Take = request.Take ?? 50,
                    Skip = request.Skip ?? 0,
                    Order = request.Order ?? "desc",
                    Field = request.Field ?? "date_created"
                };

                var searchResult = await _dossierService.SearchDossierAsync(searchQuery);

                if (!searchResult.IsSuccess)
                {
                    _logger.LogError("Both GetAllDossier and SearchDossier failed for user {0}", request.UserId);
                    return searchResult;
                }

                return searchResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dossiers for user {0}", request.UserId);
                return Result<IEnumerable<DossierSearchSanitized>>.Failure(
                    new Error(Constants.DossierFail, "Failed to retrieve dossiers"));
            }
        }
    }
}
