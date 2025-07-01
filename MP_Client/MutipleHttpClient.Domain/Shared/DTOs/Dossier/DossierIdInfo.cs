using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record DossierIdInfo(Guid DossierId, string Code, string Status, DateTime? DateCreated)
{
    [JsonIgnore]
    public int InternalId { get; init; }
};