using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record LocalDossierSanitized(Guid DossierId, Guid Id, string? Latitude, string? Longitude, string? Address,
                                            Guid? CityId, Guid? DistrictId, string? LocalComment, Guid? PropertyTypeId,
                                            string? LocalAddress, string? OpeningHours, string? OpeningDays, Guid? VisibilityId,
                                            string? Area, bool? HasSanitary, string? PropertyType, string? Facade,
                                            string? YearsOfExperience, decimal? Price, string? Zone, string? CityName,
                                            string? Region, string? DistrictName)
    {
        [JsonIgnore]
        public int InternalDossierId { get; init; }
        [JsonIgnore]
        public int InternalId { get; init; }
        [JsonIgnore]
        public int? InternalCityId { get; init; }
        [JsonIgnore]
        public int? InternalDistrictId { get; init; }
        [JsonIgnore]
        public int? InternalPropertyTypeId { get; init; }
        [JsonIgnore]
        public int? InternalVisibilityId { get; init; }
    };
}
