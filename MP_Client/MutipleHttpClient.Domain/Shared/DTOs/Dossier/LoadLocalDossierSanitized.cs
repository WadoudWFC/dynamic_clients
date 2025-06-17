using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record LoadLocalDossierSanitized(Guid DossierId, Guid Id, string? Latitude, string? Longitude, string? Address, Guid? CityId, Guid? DistrictId, string? LocalComment,
                                            Guid? PropertyTypeId, string? LocalAddress, string? OpeningHours, string? OpeningDays, Guid? VisibilityId, string? Area, bool? HasSanitary,
                                            string? PropertyType, string? Facade, string? YearsOfExperience, decimal? Price, string? Zone, string? City, string? Region, string? Disctict)
    {
        [JsonIgnore]
        public int InternalDossierId { get; set; }
        [JsonIgnore]
        public int InternalId { get; set; }
    }
}
