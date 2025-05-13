using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.Models.Gestion.Responses
{
    public class ActivityNatureResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nature_activite")]
        public string? ActivityNature { get; set; }
        [JsonPropertyName("name_activite")]
        public string? ActivityName { get; set; }
    }
}
