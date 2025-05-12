using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure;

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
