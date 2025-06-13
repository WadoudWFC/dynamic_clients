using System.Globalization;
using Microsoft.Extensions.Logging;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Comment_Management.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Demand_Management.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Requests;
using MultipleHttpClient.Application.Dossier.Command;
using MultipleHttpClient.Application.Dossier.Queries;
using MultipleHttpClient.Application.Interfaces.Dossier;
using MultipleHttpClient.Application.Interfaces.Security;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Security;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application;

public class DossierAglouService : IDossierAglouService
{
    private readonly IHttpDossierAglouService _httpDosserAglouService;
    private readonly IReferenceDataMappingService _mappingAglouDataService;
    private readonly IIdMappingService _idMappingService;
    private readonly ILogger<DossierAglouService> _logger;
    public DossierAglouService(IHttpDossierAglouService httpDosserAglouService, IReferenceDataMappingService mappingAglouDataService, ILogger<DossierAglouService> logger, IIdMappingService idMappingService)
    {
        _httpDosserAglouService = httpDosserAglouService;
        _mappingAglouDataService = mappingAglouDataService;
        _logger = logger;
        _idMappingService = idMappingService;
    }

    public async Task<Result<IEnumerable<CommentSanitized>>> GetAllCommentsAsync(GetAllCommentsQuery query)
    {
        var dossierId = _mappingAglouDataService.GetReferenceIdForGuid(query.DossierId, Constants.Dossier);
        var userId = _mappingAglouDataService.GetReferenceIdForGuid(query.UserId, Constants.User);
        if (dossierId == null || userId == null)
        {
            _logger.LogError("Failed to fetch comments for dossier {0}", query.DossierId);
            return Result<IEnumerable<CommentSanitized>>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
        }
        var request = new GetAllCommentRequestBody
        {
            DossierId = dossierId.Value,
            UserId = userId.Value
        };
        var response = await _httpDosserAglouService.GetAllCommentsAsync(request);
        if (!response.IsSuccess || response.Data?.Data == null)
        {
            _logger.LogError("Failed to fetch comments for dossier {0}", query.DossierId);
            return Result<IEnumerable<CommentSanitized>>.Failure(new Error(Constants.DossierFail, Constants.NoDataError));
        }
        // Pre-fetch user mappings
        var userIds = response.Data.Data.Select(c => c.UserCreatedId).Union(response.Data.Data.Select(c => c.Id)).Distinct();
        var userMappings = userIds.ToDictionary(id => id, id => _mappingAglouDataService.GetOrCreateGuidForReferenceId(id, Constants.User));
        // Sanitize and map response
        var comments = response.Data.Data.Select(comment =>
        {
            var commentId = _mappingAglouDataService.GetOrCreateGuidForReferenceId(comment.Id, Constants.Comment);
            return new CommentSanitized(Comment: commentId,
                                        CreationDate: DateTime.Parse(comment.CreationDate),
                                        Time: comment.Time,
                                        DossierId: query.DossierId,
                                        Message: comment.Message,
                                        UserCreatedId: userMappings[comment.UserCreatedId],
                                        User: new UserCommentSanitized(User: userMappings[comment.User.Id],
                                                                        Image: comment.User.Image,
                                                                        Align: comment.User.Align,
                                                                        FullName: comment.User.FullName)
                                        {
                                            InternalId = comment.User.Id
                                        })
            {
                InternalId = comment.Id,
                InternalDossierId = comment.DossierId,
                InternalUserCreatedId = comment.UserCreatedId
            };
        });
        _logger.LogInformation("Successfully retrieved {0} commments for dossier {1}", comments.Count(), query.DossierId);
        return Result<IEnumerable<CommentSanitized>>.Success(comments);
    }
    public async Task<Result<IEnumerable<DossierAllSanitized>>> GetAllDossierAsync(GetAllDossierQuery query)
    {
        try
        {
            var response = await _httpDosserAglouService.GetAllDossierAsync(new ProfileRoleRequestBody { RoleId = query.RoleId });
            if (!response.IsSuccess || response.Data?.Data == null)
            {
                _logger.LogError("Failed to get dossier for: {0}", query.UserId);
                return Result<IEnumerable<DossierAllSanitized>>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
            }
            if (!int.TryParse(query.RoleId, out int profileId))
            {
                _logger.LogError("Failed to get dossier for: {0}", query.UserId);
                return Result<IEnumerable<DossierAllSanitized>>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
            }
            var dossiers = response.Data.Data.Select(d =>
            {
                var accessInfo = new RoleAccessInfo(RoleId: RoleSecurity.GetRoleGuid(profileId), IsRegularUser: RoleSecurity.IsRegularUser(profileId), Expiration: DateTime.Now.AddMinutes(20));
                return new DossierAllSanitized(Dossier: _mappingAglouDataService.GetOrCreateGuidForReferenceId(d.Id, Constants.Dossier),
                                                Code: d.Code,
                                                DemandTypeId: _mappingAglouDataService.GetOrCreateGuidForReferenceId(d.DemandTypeId, Constants.Dossier),
                                                DossierStatusId: _mappingAglouDataService.GetOrCreateGuidForReferenceId(d.DossierStatusId, Constants.Dossier),
                                                Status: d.Status)
                {
                    InternalId = d.Id,
                    InternalDemandTypeId = d.DemandTypeId,
                    InternalDossierStatusId = d.DossierStatusId,
                    RoleAccessInfo = accessInfo
                };
            });
            return Result<IEnumerable<DossierAllSanitized>>.Success(dossiers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result<IEnumerable<DossierAllSanitized>>.Failure(new Error(Constants.DossierFail, ex.Message));
        }

    }
    public async Task<Result<DossierCountsSanitized>> GetCountsAsync(GetCountsQuery query)
    {
        try
        {
            var userId = _idMappingService.GetUserIdForGuid(query.UserId);
            if (userId == null)
            {
                _logger.LogError("Failed to get counts for: {0}", query.UserId);
                return Result<DossierCountsSanitized>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
            }
            var response = await _httpDosserAglouService.GetCountsAsync(userId.Value.ToString(), query.RoleId ?? string.Empty);
            if (!response.IsSuccess || response.Data?.Data == null)
            {
                _logger.LogError("Failed to get counts for: {0}", query.UserId);
                return Result<DossierCountsSanitized>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
            }
            var data = response.Data.Data;
            var debugInfo = data.DebugInfo;
            var commercialDivisionID = !string.IsNullOrEmpty(debugInfo.ComercialDivisionId) ?
                                        _mappingAglouDataService.GetOrCreateGuidForReferenceId(int.Parse(debugInfo.ComercialDivisionId),
                                                                                                Constants.CommercialDivision) : Guid.Empty;

            var sanitized = new DossierCountsSanitized(TotalDossier: data.TotalDossier,
                                                        TotalProcessedDossier: data.TotalProcessedDossiers,
                                                        TotalPendingDossier: data.TotalPendingDossiers,
                                                        DebugInfo: new DebugInfoSanitized(UserId: query.UserId,
                                                                                            CommercialDivisionId: commercialDivisionID,
                                                                                            ApplyFilter: debugInfo.ApplyFilter,
                                                                                            ConfiguredStatuses: debugInfo.ConfiguredStatuses,
                                                                                            CurrentStatusDistribution: debugInfo.CurrentStatusDistribution,
                                                                                            FinalCounts: new FinalCountsSanitized(Total: debugInfo.FinalCounts.Total,
                                                                                                                                   Treated: debugInfo.FinalCounts.Treated,
                                                                                                                                   Pending: debugInfo.FinalCounts.Pending))
                                                        {
                                                            InternalUserId = debugInfo.UserId,
                                                            InternalProfileId = debugInfo.ProfileId,
                                                            InternalCommercialDivisionId = !string.IsNullOrEmpty(debugInfo.ComercialDivisionId)
                    ? int.Parse(debugInfo.ComercialDivisionId) : null
                                                        });
            return Result<DossierCountsSanitized>.Success(sanitized);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result<DossierCountsSanitized>.Failure(new Error(Constants.DossierFail, ex.Message));
        }
    }
    public async Task<Result<IEnumerable<DossierStatusSanitized>>> GetDossierStatusAsync(GetDossierStatusQuery query)
    {
        try
        {
            var response = await _httpDosserAglouService.GetDossierStatusAsync(null);
            if (!response.IsSuccess || response.Data?.Data == null)
            {
                _logger.LogError("Failed to get dossier statuses");
                return Result<IEnumerable<DossierStatusSanitized>>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
            }
            var statues = response.Data.Data.Select(s =>
            {
                var roleInfo = new RoleInfo(RoleId: RoleSecurity.GetRoleGuid(s.Id), IsRegularUser: RoleSecurity.IsRegularUser(s.Id));
                return new DossierStatusSanitized(StatusId: _mappingAglouDataService.GetOrCreateGuidForReferenceId(s.Id, Constants.DossierStatus),
                                                                                    Label: s.Label,
                                                                                    Status: s.Status,
                                                                                    Code: s.Code,
                                                                                    Color: s.Color,
                                                                                    CanUpdate: s.CanUpdate,
                                                                                    CanUpload: s.CanUpload,
                                                                                    HaveMail: s.HaveMail,
                                                                                    MailName: s.MailName,
                                                                                    MailSubject: s.MailSubject,
                                                                                    Action: s.Action,
                                                                                    StatusCode: s.StatusCode)
                {
                    InternalId = s.Id,
                    RoleInfo = roleInfo
                };
            }
            );
            _logger.LogInformation("Successfully retrieved {0} dossier statues", statues.Count());
            return Result<IEnumerable<DossierStatusSanitized>>.Success(statues);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Fetching dossier statues");
            return Result<IEnumerable<DossierStatusSanitized>>.Failure(new Error(Constants.DossierFail, ex.Message));
        }
    }
    public async Task<Result<CommentOperationResult>> InsertCommentAsync(InsertCommentCommand command)
    {
        try
        {
            var userId = _idMappingService.GetUserIdForGuid(command.UserId);
            var dossierId = _mappingAglouDataService.GetReferenceIdForGuid(command.DossierId, Constants.Dossier);
            if (userId == null || dossierId == null)
            {
                _logger.LogError("Failed to insert comment for user: {0}", command.UserId);
                return Result<CommentOperationResult>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
            }
            var response = await _httpDosserAglouService.InsertCommentAsync(new InsertCommentRequestBody
            {
                UserId = userId.Value,
                Content = command.Content,
                DossierId = dossierId.Value
            });
            if (!response.IsSuccess || response.Data == null)
            {
                _logger.LogError("Failed to insert comment for user: {0}", command.UserId);
                return Result<CommentOperationResult>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
            }
            // Map the new comment ID
            var commentId = response.Data.ToString() != null ? _mappingAglouDataService.GetOrCreateGuidForReferenceId(int.Parse(response.Data.Data.ToString())!,
                                                                                                                        Constants.Comment) : (Guid?)null;
            return Result<CommentOperationResult>.Success(new CommentOperationResult(IsSuccess: true, Message: response.Data.Message, CommentId: commentId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result<CommentOperationResult>.Failure(new Error(Constants.DossierFail, ex.Message));
        }
    }

    public Task<Result<InsertDossierOperationResult>> InsertDossierAsync(InsertDossierCommand command)
    {
        try
        {
            // Map GUIDs to internal IDs
            var request = new InsertDossierFormBodyRequest
            {
                id_statutdossier = _mappingAglouDataService.GetReferenceIdForGuid(command.StatusId, Constants.DossierStatus)?.ToString(),
                user_id = _mappingAglouDataService.GetReferenceIdForGuid(command.UserId, Constants.User)?.ToString(),
                id_typedemende = command.DemandTypeId.HasValue ?
                    _mappingAglouDataService.GetReferenceIdForGuid(command.DemandTypeId.Value, Constants.DemandType)?.ToString() : null,
                id_natureactivite = command.ActivityNatureId.HasValue ?
                    _mappingAglouDataService.GetReferenceIdForGuid(command.ActivityNatureId.Value, Constants.ActivityNature)?.ToString() : null,
                id_region = command.RegionId.HasValue ?
                    _mappingAglouDataService.GetReferenceIdForGuid(command.RegionId.Value, Constants.Region)?.ToString() : null,
                id_ville = command.CityId.HasValue ?
                    _mappingAglouDataService.GetReferenceIdForGuid(command.CityId.Value, Constants.City)?.ToString() : null,
                id_arrandissement = command.DistrictId.HasValue ?
                    _mappingAglouDataService.GetReferenceIdForGuid(command.DistrictId.Value, Constants.District)?.ToString() : null,
                adresslocal = command.LocalAddress,
                id_visibilite = command.VisibilityId.HasValue ?
                    _mappingAglouDataService.GetReferenceIdForGuid(command.VisibilityId.Value, Constants.Visibility)?.ToString() : null,
                superficie = command.Area,
                presence_sanitaire = command.HasSanitary.HasValue ?
                    (command.HasSanitary.Value ? "1" : "0") : null,
                id_typebien = command.PropertyTypeId.HasValue ?
                    _mappingAglouDataService.GetReferenceIdForGuid(command.PropertyTypeId.Value, Constants.PropertyType)?.ToString() : null,
                prix = command.Price?.ToString(CultureInfo.InvariantCulture),
                facade = command.Facade,
                Latitude = command.Latitude,
                Longitude = command.Longitude,
                horaire_ouverture = command.OpeningHours,
                jour_ouverture = command.OpeningDays,
                anne_experience = command.YearsOfExperience,
                commentairelocal = command.LocalComment,
                ice = command.ICE,
                rc = command.RC,
                autre_ville = command.OtherCity,
                nomlocal = command.LocalName,
                nom_ag_WFC_proche = command.NearestWFCAgencyName,
                dist_bur_WFC_proche = command.NearestWFCOfficeDistance,
                dist_ag_WFC_proche = command.NearestWFCAgencyDistance,
                potentiel = command.Potential?.ToString(),
                pack = command.Pack,
                forme_juridique = command.LegalForm?.ToString(),
                regime_imposi = command.TaxRegime?.ToString(),
                mode_mandataire = command.AgentMode?.ToString(),
                identification_fiscale = command.FiscalIdentification,
                photosInterieur_0 = command.InteriorPhotos?.FirstOrDefault().ToString(),
                photosExterieur_0 = command.ExteriorPhotos?.FirstOrDefault().ToString()
            };

            // Validate all required mappings exist
            var missingMappings = new List<string>();
            if (request.id_statutdossier == null) missingMappings.Add("Status");
            if (request.user_id == null) missingMappings.Add("User");

            if (missingMappings.Any())
            {
                return Result<DossierOperationResult>.Failure(
                    new Error("MappingError", $"Missing mappings for: {string.Join(", ", missingMappings)}"));
            }

            // Call the legacy API
            var response = await _httpDossierService.InsertDossierFormAsync(request);

            if (!response.IsSuccess || response.Data == null)
            {
                return Result<DossierOperationResult>.Failure(
                    new Error("ApiError", response.ErrorMessage ?? "Failed to create dossier"));
            }

            // Map the response
            Guid? dossierId = null;
            if (response.Data.Data != null && int.TryParse(response.Data.Data.ToString(), out var newDossierId))
            {
                dossierId = _mappingAglouDataService.GetOrCreateGuidForReferenceId(newDossierId, Constants.Dossier);
                _logger.LogInformation("Created new dossier with internal ID {InternalId} and GUID {DossierId}",
                    newDossierId, dossierId);
            }

            return Result<DossierOperationResult>.Success(
                new DossierOperationResult(
                    IsSuccess: true,
                    Message: response.Data.Message ?? "Dossier created successfully",
                    DossierId: dossierId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating dossier");
            return Result<DossierOperationResult>.Failure(
                new Error("ServerError", "An error occurred while creating dossier"));
        }
    }

    public async Task<Result<IEnumerable<DossierSearchSanitized>>> SearchDossierAsync(SearchDossierQuery query)
    {
        try
        {
            var userId = _idMappingService.GetUserIdForGuid(query.UserId);
            if (userId == null)
            {
                _logger.LogError("User ID mapping not found");
                return Result<IEnumerable<DossierSearchSanitized>>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
            }

            query.InternalUserId = userId.Value;

            // Prepare request with mapped IDs
            var request = new SearchDossierRequestBody
            {
                Id = userId.Value.ToString(),
                RoleId = query.RoleId,
                ApplyFilter = query.ApplyFilter,
                Code = query.Code,
                DosseriStatusId = query.DossierStatusId.HasValue ?
                    _mappingAglouDataService.GetReferenceIdForGuid(query.DossierStatusId.Value, Constants.DStatus)?.ToString() : null,
                DemandTypeId = query.DemandTypeId.HasValue ?
                    _mappingAglouDataService.GetReferenceIdForGuid(query.DemandTypeId.Value, Constants.Demand)?.ToString() : null,
                NatureOfActivityId = query.NatureActivityId.HasValue ?
                    _mappingAglouDataService.GetReferenceIdForGuid(query.NatureActivityId.Value, Constants.Activity)?.ToString() : null,
                PartnerId = query.PartnerId.HasValue ?
                    _mappingAglouDataService.GetReferenceIdForGuid(query.PartnerId.Value, Constants.Partner)?.ToString() : null,
                CommercialCuttingId = query.CommercialCuttingId.HasValue ?
                    _mappingAglouDataService.GetReferenceIdForGuid(query.CommercialCuttingId.Value, Constants.Decoupage)?.ToString() : null,
                StartDate = query.StartDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                EndDate = query.EndDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                TakeNumber = query.Take,
                SkipNumber = query.Skip,
                Field = query.Field,
                Order = query.Order
            };

            // Validate that all provided GUIDs were successfully mapped
            var missingMappings = new List<string>();
            if (query.DossierStatusId.HasValue && request.DosseriStatusId == null) missingMappings.Add("DossierStatus");
            if (query.DemandTypeId.HasValue && request.DemandTypeId == null) missingMappings.Add("DemandType");
            if (query.NatureActivityId.HasValue && request.NatureOfActivityId == null) missingMappings.Add("NatureActivity");
            if (query.PartnerId.HasValue && request.PartnerId == null) missingMappings.Add("Partner");
            if (query.CommercialCuttingId.HasValue && request.CommercialCuttingId == null) missingMappings.Add("CommercialCutting");

            if (missingMappings.Any())
            {
                _logger.LogError("Invalid Reference mappings. Failed to retrieve dossiers");
                return Result<IEnumerable<DossierSearchSanitized>>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
            }

            var response = await _httpDosserAglouService.SearchDossier(request);

            if (!response.IsSuccess || response.Data?.Data == null)
            {
                _logger.LogError("Failed to retrieve dossiers");
                return Result<IEnumerable<DossierSearchSanitized>>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
            }

            // Pre-fetch all necessary mappings for better performance
            var dossierIds = response.Data.Data.Select(d => d.Id).Distinct();
            var dossierMappings = dossierIds.ToDictionary(
                id => id,
                id => _mappingAglouDataService.GetOrCreateGuidForReferenceId(id, Constants.Dossier));

            var natureActivityIds = response.Data.Data
                .Where(d => d.Id_NatureActivite.HasValue)
                .Select(d => d.Id_NatureActivite.Value)
                .Distinct();
            var natureActivityMappings = natureActivityIds.ToDictionary(
                id => id,
                id => _mappingAglouDataService.GetOrCreateGuidForReferenceId(id, Constants.Activity));

            var demandTypeIds = response.Data.Data.Select(d => d.Id_TypeDemande).Distinct();
            var demandTypeMappings = demandTypeIds.ToDictionary(
                id => id,
                id => _mappingAglouDataService.GetOrCreateGuidForReferenceId(id, Constants.Demand));

            var commercialCuttingIds = response.Data.Data.Select(d => d.Id_DecoupageCommercial).Distinct();
            var commercialCuttingMappings = commercialCuttingIds.ToDictionary(
                id => id,
                id => _mappingAglouDataService.GetOrCreateGuidForReferenceId(id, Constants.Decoupage));

            var dossierStatusIds = response.Data.Data.Select(d => d.Id_StatutDossier).Distinct();
            var dossierStatusMappings = dossierStatusIds.ToDictionary(
                id => id,
                id => _mappingAglouDataService.GetOrCreateGuidForReferenceId(id, Constants.DStatus));

            // Map and sanitize the response
            var results = response.Data.Data.Select(d =>
            {
                var localDossier = d.LocalDossier != null ? new LocalDossierSanitized(
                    DossierId: dossierMappings[d.LocalDossier.Id_Dossier],
                    Id: _mappingAglouDataService.GetOrCreateGuidForReferenceId(d.LocalDossier.Id, Constants.LocalDossier),
                    Latitude: d.LocalDossier.Latitude,
                    Longitude: d.LocalDossier.Longitude,
                    Address: d.LocalDossier.Address,
                    Zone: d.LocalDossier.Zone,
                    City: d.LocalDossier.City,
                    DecopageCMR: d.LocalDossier.DecopageCMR,
                    Region: d.LocalDossier.Region)
                {
                    InternalDossierId = d.LocalDossier.Id_Dossier,
                    InternalId = d.LocalDossier.Id
                } : null;

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
                    DateCreated: !string.IsNullOrEmpty(d.DateCreated) ? DateTime.Parse(d.DateCreated) : null)
                {
                    InternalId = d.Id,
                    InternalNatureActivityId = d.Id_NatureActivite,
                    InternalDemandTypeId = d.Id_TypeDemande,
                    InternalCommercialCuttingId = d.Id_DecoupageCommercial,
                    InternalDossierStatusId = d.Id_StatutDossier
                };
            });

            _logger.LogInformation("Successfully retrieved {Count} dossiers for user {UserId}", results.Count(), query.UserId);
            return Result<IEnumerable<DossierSearchSanitized>>.Success(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching dossiers for user {UserId}", query.UserId);
            return Result<IEnumerable<DossierSearchSanitized>>.Failure(new Error(Constants.DossierFail, ex.Message));
        }
    }
    public async Task<Result<IEnumerable<HistorySearchSanitized>>> SearchHistroyAsync(SearchHistoryQuery query)
    {
        try
        {
            var dossierId = _mappingAglouDataService.GetReferenceIdForGuid(query.DossierId, Constants.Dossier);
            if (dossierId == null)
            {
                return Result<IEnumerable<HistorySearchSanitized>>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
            }
            var request = new HistorySearchRequestBody
            {
                Field = query.Field,
                DossierId = dossierId.Value,
                Order = query.Order,
                SkipNumber = query.Skip,
                TakeNumber = query.Take
            };
            var response = await _httpDosserAglouService.SearchHistroyAsync(request);
            if (!response.IsSuccess || response.Data?.Data == null)
            {
                return Result<IEnumerable<HistorySearchSanitized>>.Failure(new Error(Constants.DossierFail, "Failed to search history"));
            }
            var statusIds = response.Data.Data.SelectMany(h => new[] { h.PreviousStatusId, h.NextStatusId }).Distinct();
            var statusMappings = statusIds.ToDictionary(
                id => id,
                id => _mappingAglouDataService.GetOrCreateGuidForReferenceId(id, Constants.DStatus));
            var results = response.Data.Data.Select(h => new HistorySearchSanitized(HistorySearch: _mappingAglouDataService.GetOrCreateGuidForReferenceId(h.Id, Constants.History),
                                                                                    CreationDate: DateTime.Parse(h.CreationDate),
                                                                                    PreviousStatusId: statusMappings[h.PreviousStatusId],
                                                                                    NextStatusId: statusMappings[h.NextStatusId],
                                                                                    DossierId: _mappingAglouDataService.GetOrCreateGuidForReferenceId(h.DossierId, Constants.Dossier),
                                                                                    DossierNumber: h.DossierNumber,
                                                                                    PreviousStatusText: h.PreviousStatusText,
                                                                                    NextStatusText: h.NextStatusText,
                                                                                    Operator: h.Operator)
            {
                InternalId = h.Id,
                InternalPreviousStatusId = h.PreviousStatusId,
                InternalNextStatusId = h.NextStatusId,
                InternalDossierId = h.DossierId
            });
            return Result<IEnumerable<HistorySearchSanitized>>.Success(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Searching History");
            return Result<IEnumerable<HistorySearchSanitized>>.Failure(new Error(Constants.DossierFail, ex.Message));
        }
    }
    public async Task<Result<DossierUpdateResult>> UpdateDossierAsync(UpdateDossierCommand command)
    {
        try
        {
            // Convert GUIDs to internal IDs with null checks
            var dossierId = _mappingAglouDataService.GetReferenceIdForGuid(command.DossierId, Constants.Dossier);
            var statusId = _mappingAglouDataService.GetReferenceIdForGuid(command.StatusId, Constants.DossierStatus);
            var demandTypeId = _mappingAglouDataService.GetReferenceIdForGuid(command.DemandTypeId, Constants.DemandType);
            var partnerId = _mappingAglouDataService.GetReferenceIdForGuid(command.PartnerId, Constants.Partner);
            var natureActivityId = _mappingAglouDataService.GetReferenceIdForGuid(command.NatureActivityId, Constants.Activity);
            var villeId = _mappingAglouDataService.GetReferenceIdForGuid(command.VilleId, Constants.City);
            var arrondissementId = _mappingAglouDataService.GetReferenceIdForGuid(command.ArrondissementId, Constants.Arrondissement);
            var typeBienId = _mappingAglouDataService.GetReferenceIdForGuid(command.TypeBienId, Constants.BienType);
            var visibiliteId = _mappingAglouDataService.GetReferenceIdForGuid(command.VisibiliteId, Constants.Visibility);
            var userId = _idMappingService.GetUserIdForGuid(command.UserId);

            // Validate all IDs were mapped
            var missingMappings = new List<string>();
            if (dossierId == null) missingMappings.Add("Dossier");
            if (statusId == null) missingMappings.Add("Status");
            if (demandTypeId == null) missingMappings.Add("DemandType");
            if (partnerId == null) missingMappings.Add("Partner");
            if (natureActivityId == null) missingMappings.Add("NatureActivity");
            if (villeId == null) missingMappings.Add("Ville");
            if (arrondissementId == null) missingMappings.Add("Arrondissement");
            if (typeBienId == null) missingMappings.Add("TypeBien");
            if (visibiliteId == null) missingMappings.Add("Visibility");
            if (userId == null) missingMappings.Add("User");

            if (missingMappings.Any())
            {
                return Result<DossierUpdateResult>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
            }

            var request = new UpdateDossierRequestBody
            {
                Id = dossierId.Value,
                LocalId = command.InternalLocalId,
                IdStatutDossier = statusId.Value,
                IdTypeDemende = demandTypeId.Value,
                IdPartenaire = partnerId.Value,
                IdNatureActivite = natureActivityId.Value,
                IdVille = villeId.Value,
                IdArrondissement = arrondissementId.Value,
                IdTypeBien = typeBienId.Value,
                IdVisibilite = visibiliteId.Value,
                AddressLocal = command.AddressLocal,
                Superficie = command.Superficie,
                PresenceSanitaire = command.PresenceSanitaire,
                Prix = command.Prix,
                Facade = command.Facade,
                Latitude = command.Latitude,
                Longitude = command.Longitude,
                HoraireOuverture = command.HoraireOuverture,
                JourOuverture = command.JourOuverture,
                AnneExperience = command.AnneExperience,
                CommentaireLocal = command.CommentaireLocal,
                Ice = command.Ice,
                Rc = command.Rc,
                AutreVille = command.AutreVille,
                NomLocal = command.NomLocal,
                NomAgWfcProche = command.NomAgWfcProche,
                DistBurWfcProche = command.DistBurWfcProche,
                DistAgWfcProche = command.DistAgWfcProche,
                Potentiel = command.Potentiel,
                Pack = command.Pack,
                FormeJuridique = command.FormeJuridique,
                RegimeImposi = command.RegimeImposi,
                ModeMandataire = command.ModeMandataire,
                IdentificationFiscale = command.IdentificationFiscale,
                UserId = userId.Value.ToString()
            };

            var response = await _httpDosserAglouService.UpdateDossierAsync(request);

            if (!response.IsSuccess || response.Data == null)
            {
                return Result<DossierUpdateResult>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
            }

            return Result<DossierUpdateResult>.Success(new DossierUpdateResult(
                IsSuccess: true,
                Message: response.Data.Message)
            {
                InternalDossierId = dossierId.Value
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating dossier {DossierId}", command.DossierId);
            return Result<DossierUpdateResult>.Failure(new Error("ServerError", ex.Message));
        }
    }
}
