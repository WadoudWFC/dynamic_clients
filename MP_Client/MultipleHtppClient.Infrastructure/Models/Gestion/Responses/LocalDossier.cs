using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure;

public class LocalDossier
{
    [JsonPropertyName("id_dossier")]
    public int IdDossier { get; set; }
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("Latitude")]
    public string Latitude { get; set; } = string.Empty;
    [JsonPropertyName("Longitude")]
    public string Longitude { get; set; } = string.Empty;
    [JsonPropertyName("adress")]
    public string Address { get; set; } = string.Empty;
    [JsonPropertyName("zon")]
    public string? Zon { get; set; }
    [JsonPropertyName("ville")]
    public string Ville { get; set; } = string.Empty;
    [JsonPropertyName("decopagecmr")]
    public string DecopageCmr { get; set; } = string.Empty;
    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;
}
