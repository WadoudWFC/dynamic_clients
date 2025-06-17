using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record SearchLocalDossierSanitied(Guid DossierId, Guid Id, string? Latitude, string? Longitude, string? Address, string? Zone, string? City, string? DecopageCMR, string? Region)
    {
        [JsonIgnore]
        public int InternalDossierId { get; init; }

        [JsonIgnore]
        public int InternalId { get; init; }
    }
}
