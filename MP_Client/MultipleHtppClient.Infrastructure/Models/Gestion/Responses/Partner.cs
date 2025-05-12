using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.Models.Gestion.Responses
{
    public class Partner
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nom")]
        public string FirstName { get; set; }
        [JsonPropertyName("prenom")]
        public string LastName { get; set; }
        [JsonPropertyName("Code")]
        public string Code { get; set; }
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }
    }
}
