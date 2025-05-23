using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;

public class LocalDossier
{
    [JsonPropertyName("id_dossier")]
    public int Id_Dossier { get; set; }
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("Latitude")]
    public string? Latitude { get; set; }
    [JsonPropertyName("Longitude")]
    public string? Longitude { get; set; }
    [JsonPropertyName("adress")]
    public string? Address { get; set; }
    [JsonPropertyName("zon")]
    public string? Zone { get; set; }
    [JsonPropertyName("ville")]
    public string? City { get; set; }
    [JsonPropertyName("decopagecmr")]
    public string? DecopageCMR { get; set; }
    [JsonPropertyName("region")]
    public string? Region { get; set; }
}
