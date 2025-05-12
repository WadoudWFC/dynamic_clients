using System.Text.Json.Serialization;
using MutipleHttpClient.Domain;

namespace MultipleHtppClient.Infrastructure;

public class DossierCountRequest
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
    [JsonPropertyName("id_role")]
    public string RoleId { get; set; }
    [JsonPropertyName("appliquefilter")]
    public bool ApplyFilter { get; set; }
}
