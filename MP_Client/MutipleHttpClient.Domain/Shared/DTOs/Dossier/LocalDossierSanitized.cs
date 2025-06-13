using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record LocalDossierSanitized(Guid DossierId,Guid Id,string? Latitude,string? Longitude,string? Address,string? Zone,string? City,string? DecopageCMR,string? Region)
    {
        [JsonIgnore]
        public int InternalDossierId { get; init; }

        [JsonIgnore]
        public int InternalId { get; init; }
    }
}
