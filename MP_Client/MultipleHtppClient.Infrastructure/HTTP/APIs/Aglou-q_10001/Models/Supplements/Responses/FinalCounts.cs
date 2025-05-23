using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;

public class FinalCounts
{
    [JsonPropertyName("Total")]
    public int Total { get; set; }
    [JsonPropertyName("Treated")]
    public int Treated { get; set; }
    [JsonPropertyName("Pending")]
    public int Pending { get; set; }
}
