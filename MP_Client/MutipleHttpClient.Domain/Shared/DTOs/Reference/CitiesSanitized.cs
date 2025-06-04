using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record CitiesSanitized(Guid City, string? Label, Guid? Region, Guid? DecoupageCommercial, string? LocalityType, string? Logo)
{
    [JsonIgnore]
    public int? CityId { get; init; }
    [JsonIgnore]
    public int? RegionId { get; init; }
    [JsonIgnore]
    public int? DecoupageCommercialId { get; init; }
}
