using System.Text.Json.Serialization;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;
namespace MutipleHttpClient.Domain;

public record DebugInfoSanitized(Guid UserId, Guid? DecoupageCommercialId, bool AppliqueFilter, List<int> ConfiguredStatuses, List<object> CurrentStatusDistribution, FinalCountsSanitized FinalCounts)
{
    [JsonIgnore]
    public ProfileType ProfileType { get; init; }
    [JsonIgnore]
    public int InternalUserId { get; init; }
    [JsonIgnore]
    public int? InternalCommercialDivisionId { get; init; }
    [JsonIgnore]
    public int InternalProfileId { get; init; }
}
