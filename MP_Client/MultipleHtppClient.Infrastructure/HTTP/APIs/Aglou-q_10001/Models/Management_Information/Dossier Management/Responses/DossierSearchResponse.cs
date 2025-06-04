using System.Text.Json.Serialization;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Responses;

public class DossierSearchResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("Code")]
    public string? Code { get; set; }
    [JsonPropertyName("id_natureactivite")]
    public int? Id_NatureActivite { get; set; }
    [JsonPropertyName("id_typedemende")]
    public int Id_TypeDemande { get; set; }
    [JsonPropertyName("id_decoupagecommercial")]
    public int Id_DecoupageCommercial { get; set; }
    [JsonPropertyName("decopagecmr")]
    public string? DecopageCMR { get; set; }
    [JsonPropertyName("id_statutdossier")]
    public int Id_StatutDossier { get; set; }
    [JsonPropertyName("canupdate")]
    public bool CanUpdate { get; set; }
    [JsonPropertyName("typedemende")]
    public string? TypeDemande { get; set; }
    [JsonPropertyName("natureactivite")]
    public string? NatureActivite { get; set; }
    [JsonPropertyName("partenaire")]
    public string? Partenaire { get; set; }
    [JsonPropertyName("LocalDossier")]
    public LocalDossier? LocalDossier { get; set; }
    [JsonPropertyName("statut")]
    public string? Statut { get; set; }
    [JsonPropertyName("labelstatut")]
    public string? LabelStatut { get; set; }
    [JsonPropertyName("date_created")]
    public string? DateCreated { get; set; }
}
