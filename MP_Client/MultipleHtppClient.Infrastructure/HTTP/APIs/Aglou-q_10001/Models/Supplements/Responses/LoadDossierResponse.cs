using System.Text.Json.Serialization;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Partner_Management.Responses;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;

public class LoadDossierResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("id_natureactivite")]
    public int? ActivityNatureId { get; set; }
    [JsonPropertyName("id_typedemende")]
    public int? RequestTypeId { get; set; }
    [JsonPropertyName("Code")]
    public string? Code { get; set; }
    [JsonPropertyName("id_Partenaire")]
    public int? PartnerId { get; set; }
    [JsonPropertyName("id_statutdossier")]
    public int? StatusId { get; set; }
    [JsonPropertyName("Commentaire")]
    public List<Commentaire> Comments { get; set; } = new List<Commentaire>();
    [JsonPropertyName("natureactivite")]
    public object? ActivityNature { get; set; }
    [JsonPropertyName("nameactivite")]
    public object? ActivityName { get; set; }
    [JsonPropertyName("typedemende")]
    public string? RequestType { get; set; }
    [JsonPropertyName("codestatut")]
    public string? StatusCode { get; set; }
    [JsonPropertyName("labelstatut")]
    public string? StatusLabel { get; set; }
    [JsonPropertyName("statut")]
    public string? Status { get; set; }
    [JsonPropertyName("canupload")]
    public bool CanUpload { get; set; }
    [JsonPropertyName("LocalDossier")]
    public LocalDossierData? LocalDossier { get; set; }
    [JsonPropertyName("Partenaire")]
    public Partner? Partner { get; set; }
    [JsonPropertyName("Historique")]
    public List<HistoryItem> History { get; set; } = new List<HistoryItem>();
}
