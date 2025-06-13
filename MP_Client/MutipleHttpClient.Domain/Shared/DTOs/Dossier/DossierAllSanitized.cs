using System.Text.Json.Serialization;
using MutipleHttpClient.Domain.Security;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record DossierAllSanitized(Guid Dossier, string Code, Guid DemandTypeId, Guid DossierStatusId, string Status)
    {
        [JsonIgnore]
        public int InternalId { get; init; }
        [JsonIgnore]
        public int InternalDemandTypeId { get; init; }
        [JsonIgnore]
        public int InternalDossierStatusId { get; init; }
        [JsonIgnore]
        public RoleAccessInfo? RoleAccessInfo { get; init; }
    }
}
