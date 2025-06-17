using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record LoadDossierResponseSanitized(Guid DossierId, Guid? ActivityNatureId, Guid? RequestTypeId, string? Code, Guid? PartnerId, Guid? StatusId,
                                                List<CommentaireSanitized> Comments, string? RequestType, string? StatusCode, string? StatusLabel, string? Status,
                                                bool CanUpload, LocalDossierSanitized? LocalDossier, PartnerSanitized? Partner, List<HistoryItemSanitized> History)
    {
        [JsonIgnore]
        public int InternalId { get; init; }
        [JsonIgnore]
        public int? InternalActivityNatureId { get; init; }
        [JsonIgnore]
        public int? InternalRequestTypeId { get; init; }
        [JsonIgnore]
        public int? InternalPartnerId { get; init; }
        [JsonIgnore]
        public int? InternalStatusId { get; init; }
    }
}
