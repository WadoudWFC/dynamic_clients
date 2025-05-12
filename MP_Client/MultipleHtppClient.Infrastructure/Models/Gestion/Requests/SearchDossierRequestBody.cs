using System.Text.Json.Serialization;
using MultipleHtppClient.Domain;

namespace MultipleHtppClient.Infrastructure;

public class SearchDossierRequestBody
{
    [JsonPropertyName("user_id")]
    public int Id { get; set; }
    [JsonPropertyName("id_role")]
    public int RoleId { get; set; }
    [JsonPropertyName("appliquefilter")]
    public bool ApplyFilter { get; set; }
    [JsonPropertyName("Code")]
    public string? Code { get; set; }
    [JsonPropertyName("id_statutdossier")]
    public int DosseriStatusId { get; set; }
    [JsonPropertyName("id_typedemende")]
    public int DemandTypeId { get; set; }
    [JsonPropertyName("skip")]
    public int SkipNumber { get; set; }
    [JsonPropertyName("take")]
    public int TakeNumber { get; set; }
    [JsonPropertyName("id_partenaire")]
    public int PartnerId { get; set; }
    [JsonPropertyName("id_natureactivite")]
    public int NatureOfActivityId { get; set; }
    [JsonPropertyName("id_decoupagecommercial")]
    public int CommercialCuttingId { get; set; }
    [JsonPropertyName("field")]
    public string? Field { get; set; }
    [JsonPropertyName("datedebut")]
    public DateTime? StartDate { get; set; }
    [JsonPropertyName("datefin")]
    public DateTime? EndDate { get; set; }
    [JsonPropertyName("order")]
    public Order? Order { get; set; }

}
