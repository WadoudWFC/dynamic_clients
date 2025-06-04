using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record PartnerTypeSanitized(Guid PartnerType, string? Label, string? Description)
{
    [JsonIgnore]
    public int InternalId { get; set; }
}
