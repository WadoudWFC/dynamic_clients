using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;

public class DebugInfo
{
    [JsonPropertyName("ProfileId")]
    public int ProfileId { get; set; }
    [JsonPropertyName("UserId")]
    public int UserId { get; set; }
    [JsonPropertyName("DecoupageCommercialId")]
    public string? ComercialDivisionId { get; set; }
    [JsonPropertyName("AppliqueFilter")]
    public bool ApplyFilter { get; set; }
    [JsonPropertyName("ConfiguredStatuses")]
    public List<int> ConfiguredStatuses { get; set; } = new List<int>();
    [JsonPropertyName("CurrentStatusDistribution")]
    public List<object> CurrentStatusDistribution { get; set; } = new List<object>();
    [JsonPropertyName("FinalCounts")]
    public FinalCounts FinalCounts { get; set; }
}
