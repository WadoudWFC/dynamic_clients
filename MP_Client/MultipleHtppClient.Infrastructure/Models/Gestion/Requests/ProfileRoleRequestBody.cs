using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.Models.Gestion.Requests
{
    public class ProfileRoleRequestBody
    {
        [JsonPropertyName("id_role")]
        public string? RoleId { get; set; }
    }
}
