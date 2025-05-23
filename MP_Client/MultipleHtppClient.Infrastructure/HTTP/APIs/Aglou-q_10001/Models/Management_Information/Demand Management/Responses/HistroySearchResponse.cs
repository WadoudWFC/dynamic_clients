using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Demand_Management.Responses;

public class HistroySearchResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("date_created")]
    public string? CreationDate { get; set; }
    [JsonPropertyName("id_statutprecedent")]
    public int PreviousStatusId { get; set; }
    [JsonPropertyName("id_statutsuivant")]
    public int NextStatusId { get; set; }
    [JsonPropertyName("id_dossier")]
    public int DossierId { get; set; }
    [JsonPropertyName("dossier")]
    public string? DossierNumber { get; set; }
    [JsonPropertyName("statutprecedent")]
    public string? PreviousStatusText { get; set; }
    [JsonPropertyName("statutsuivant")]
    public string? NextStatusText { get; set; }
    [JsonPropertyName("statutprecedentin")]
    public string? InternalPreviousStatus { get; set; }
    [JsonPropertyName("statutsuivantin")]
    public string? InternalNextStatus { get; set; }
    [JsonPropertyName("opperateur")]
    public string? Operator { get; set; }
}
