using MediatR;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Command
{
    public record InsertDossierCommand(Guid StatusId, Guid UserId, Guid? DemandTypeId, Guid? ActivityNatureId, Guid? RegionId,
                                        Guid? CityId, Guid? DistrictId, string? LocalAddress, Guid? VisibilityId, string? Area,
                                        bool? HasSanitary, Guid? PropertyTypeId, decimal? Price, string? Facade, string? Latitude,
                                        string? Longitude, string? OpeningHours, string? OpeningDays, string? YearsOfExperience,
                                        string? LocalComment, string? ICE, string? RC, string? OtherCity, string? LocalName,
                                        string? NearestWFCAgencyName, string? NearestWFCOfficeDistance, string? NearestWFCAgencyDistance,
                                        int? Potential, string? Pack, int? LegalForm, int? TaxRegime, int? AgentMode,
                                        string? FiscalIdentification, List<string>? InteriorPhotos, List<string>? ExteriorPhotos)
        : IRequest<Result<InsertDossierOperationResult>>;
}
