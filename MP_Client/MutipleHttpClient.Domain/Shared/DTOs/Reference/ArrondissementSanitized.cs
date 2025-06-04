using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record ArrondissementSanitized(Guid Arrondissement, string? Label)
{
    [JsonIgnore]
    public int InternalId { get; init; }
}
