using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record DossierSearchSanitized(Guid DossierId, string? Code, Guid? NatureActivityId, Guid DemandTypeId,
                                        Guid CommercialCuttingId, Guid DossierStatusId, bool CanUpdate, string? TypeDemande,
                                        string? ActivityNature, string? Partner, SearchLocalDossierSanitied? LocalDossierSanitized,
                                        string? Status, string? LabelStatus, DateTime? DateCreated)
    {
        [JsonIgnore]
        public int InternalId { get; init; }
        [JsonIgnore]
        public int? InternalNatureActivityId { get; init; }
        [JsonIgnore]
        public int InternalDemandTypeId { get; init; }
        [JsonIgnore]
        public int InternalCommercialCuttingId { get; init; }
        [JsonIgnore]
        public int InternalDossierStatusId { get; init; }
    }
}
