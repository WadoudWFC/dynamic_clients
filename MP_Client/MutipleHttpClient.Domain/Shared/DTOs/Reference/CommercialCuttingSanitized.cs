using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record CommercialCuttingSanitized(Guid CommercialCutting, string? Label)
{
    [JsonIgnore]
    public int InternalId { get; set; }
}
