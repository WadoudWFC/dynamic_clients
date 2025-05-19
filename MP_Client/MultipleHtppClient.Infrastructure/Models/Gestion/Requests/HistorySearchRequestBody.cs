using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure;

public class HistorySearchRequestBody
{
    [JsonPropertyName("field")]
    public string Field { get; set; } = string.Empty;
    [JsonPropertyName("id_dossier")]
    public int DossierId { get; set; }
    [JsonPropertyName("order")]
    public string Order { get; set; }
    [JsonPropertyName("skip")]
    public int SkipNumber { get; set; }
    [JsonPropertyName("take")]
    public int TakeNumber { get; set; }
}
