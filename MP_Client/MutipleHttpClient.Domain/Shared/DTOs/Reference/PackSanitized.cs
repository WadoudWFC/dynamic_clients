using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record PackSanitized(Guid Pack, string? Label, string? ArabicLabel, string? Description)
{
    [JsonIgnore]
    public int InternalId { get; set; }
}
