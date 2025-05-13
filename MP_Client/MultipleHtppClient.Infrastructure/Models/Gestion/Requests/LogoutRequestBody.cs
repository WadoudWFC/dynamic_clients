using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.Models.Gestion.Requests
{
    public class LogoutRequestBody
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;
    }
}
