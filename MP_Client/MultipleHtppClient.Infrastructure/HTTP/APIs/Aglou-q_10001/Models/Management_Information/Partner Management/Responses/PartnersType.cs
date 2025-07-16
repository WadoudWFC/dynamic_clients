using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Partner_Management.Responses
{
    public class PartnersType
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
