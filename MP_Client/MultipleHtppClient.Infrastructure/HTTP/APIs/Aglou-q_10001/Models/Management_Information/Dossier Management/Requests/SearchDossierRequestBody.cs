using System.Text.Json.Serialization;
using MutipleHttpClient.Domain.Converters;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Requests;

public class SearchDossierRequestBody
{
    [JsonPropertyName("user_id")]
    public string Id { get; set; }
    [JsonPropertyName("id_role")]
    public string RoleId { get; set; }
    [JsonPropertyName("appliquefilter")]
    public bool ApplyFilter { get; set; }
    [JsonPropertyName("Code")]
    public string? Code { get; set; }
    [JsonPropertyName("id_statutdossier")]
    public string? DosseriStatusId { get; set; }
    [JsonPropertyName("id_typedemende")]
    public string? DemandTypeId { get; set; }
    [JsonPropertyName("id_natureactivite")]
    public string? NatureOfActivityId { get; set; }
    [JsonPropertyName("id_partenaire")]
    public string? PartnerId { get; set; }
    [JsonPropertyName("id_decoupagecommercial")]
    public string? CommercialCuttingId { get; set; }
    [JsonPropertyName("datedebut")]
    public string? StartDate { get; set; }
    [JsonPropertyName("datefin")]
    public string? EndDate { get; set; }
    [JsonPropertyName("take")]
    public int TakeNumber { get; set; }
    [JsonPropertyName("skip")]
    public int SkipNumber { get; set; }
    [JsonPropertyName("field")]
    public string? Field { get; set; } = "code";
    [JsonPropertyName("order")]
    public string? Order { get; set; }
}
