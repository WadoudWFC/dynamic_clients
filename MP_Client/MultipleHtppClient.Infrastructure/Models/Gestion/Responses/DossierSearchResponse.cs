using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure;

public class DossierSearchResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("Code")]
    public string Code { get; set; } = string.Empty;
    [JsonPropertyName("id_natureactivite")]
    public int? IdNatureActivite { get; set; }
    [JsonPropertyName("id_typedemende")]
    public int IdTypeDemande { get; set; }
    [JsonPropertyName("id_decoupagecommercial")]
    public int IdDecoupageCommercial { get; set; }
    [JsonPropertyName("decopagecmr")]
    public string DecopageCmr { get; set; } = string.Empty;
    [JsonPropertyName("id_statutdossier")]
    public int IdStatutDossier { get; set; }
    [JsonPropertyName("canupdate")]
    public bool CanUpdate { get; set; }
    [JsonPropertyName("typedemende")]
    public string TypeDemande { get; set; } = string.Empty;
    [JsonPropertyName("natureactivite")]
    public string? NatureActivite { get; set; }
    [JsonPropertyName("partenaire")]
    public string Partenaire { get; set; } = string.Empty;
    [JsonPropertyName("LocalDossier")]
    public LocalDossier LocalDossier { get; set; } = new LocalDossier();
    [JsonPropertyName("statut")]
    public string Statut { get; set; } = string.Empty;
    [JsonPropertyName("labelstatut")]
    public string LabelStatut { get; set; } = string.Empty;
    [JsonPropertyName("date_created")]
    public DateTime DateCreated { get; set; }
}
