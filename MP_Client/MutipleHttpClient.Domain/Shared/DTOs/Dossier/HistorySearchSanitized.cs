using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record HistorySearchSanitized(Guid HistorySearch, DateTime CreationDate, Guid PreviousStatusId, Guid NextStatusId, Guid DossierId, string DossierNumber, string PreviousStatusText, string NextStatusText, string Operator)
{
    [JsonIgnore]
    public int InternalId { get; init; }
    [JsonIgnore]
    public int InternalPreviousStatusId { get; init; }
    [JsonIgnore]
    public int InternalNextStatusId { get; init; }
    [JsonIgnore]
    public int InternalDossierId { get; init; }
}
