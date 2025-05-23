using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Requests
{
    public class ProfileRoleRequestBody
    {
        [JsonPropertyName("id_role")]
        public string? RoleId { get; set; }
    }
}
