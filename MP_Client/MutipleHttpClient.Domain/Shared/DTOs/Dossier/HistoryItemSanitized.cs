using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record HistoryItemSanitized(Guid HistoryId, Guid DossierId, Guid? PreviousStatusId, Guid? NextStatusId,
                                        DateTime? CreationDate, string? PreviousStatusText, string? NextStatusText, string? Operator)
    {
        [JsonIgnore]
        public int InternalId { get; init; }
        [JsonIgnore]
        public int InternalDossierId { get; init; }
        [JsonIgnore]
        public int? InternalPreviousStatusId { get; init; }
        [JsonIgnore]
        public int? InternalNextStatusId { get; init; }
    };
}
