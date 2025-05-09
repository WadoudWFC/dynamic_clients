using System.Text.Json.Serialization;

namespace MultipleHtppClient.API;

public class FinalCounts
{
    [JsonPropertyName("Total")]
    public int Total { get; set; }
    [JsonPropertyName("Treated")]
    public int Treated { get; set; }
    [JsonPropertyName("Pending")]
    public int Pending { get; set; }
}
