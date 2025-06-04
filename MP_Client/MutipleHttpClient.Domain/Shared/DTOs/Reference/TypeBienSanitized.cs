using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record TypeBienSanitized(Guid TypeBien, string? Label)
{
    [JsonIgnore]
    public int InternalId { get; set; }
}
