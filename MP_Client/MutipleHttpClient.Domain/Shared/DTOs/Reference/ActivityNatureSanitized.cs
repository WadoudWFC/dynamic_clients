using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record ActivityNatureSanitized(Guid Id, string? ActivityNature, string? ActivityName)
{
    [JsonIgnore]
    public int? InternalId { get; init; }
}
