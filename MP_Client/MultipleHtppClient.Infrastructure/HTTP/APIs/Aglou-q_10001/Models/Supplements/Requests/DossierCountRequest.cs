using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Requests;

public class DossierCountRequest
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
    [JsonPropertyName("id_role")]
    public string RoleId { get; set; }
    [JsonPropertyName("appliquefilter")]
    public bool ApplyFilter { get; set; }
}
