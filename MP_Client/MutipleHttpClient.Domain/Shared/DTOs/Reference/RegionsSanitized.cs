using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record RegionsSanitized(Guid Region, string? Label, string? Logo, string? Map)
{
    [JsonIgnore]
    public int InternalId { get; init; }
}
