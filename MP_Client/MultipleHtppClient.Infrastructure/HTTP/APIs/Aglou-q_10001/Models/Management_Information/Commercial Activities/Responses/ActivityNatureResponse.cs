using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Commercial_Activities.Responses
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
