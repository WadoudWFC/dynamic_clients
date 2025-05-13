using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.Models.Gestion.Responses
{
    public class PackResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("label")]
        public string? Label { get; set; }
        [JsonPropertyName("arblabel")]
        public string? ArabicLabel { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
