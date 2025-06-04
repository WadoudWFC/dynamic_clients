using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record DemandTypeSanitized(Guid DemandType, string? Label, string? Description)
{
    [JsonIgnore]
    public int InternalId { get; set; }
}
