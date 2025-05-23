using System.Text.Json.Serialization;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Statistics_and_Counts.Responses;

public class DossierCounts
{
    [JsonPropertyName("TotalDossier")]
    public int TotalDossier { get; set; }
    [JsonPropertyName("TotalDossierTraiter")]
    public int TotalProcessedDossiers { get; set; }
    [JsonPropertyName("TotalDossierEncours")]
    public int TotalPendingDossiers { get; set; }
    [JsonPropertyName("DebugInfo")]
    public DebugInfo DebugInfo { get; set; }
}
