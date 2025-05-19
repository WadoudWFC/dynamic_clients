using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.Models.Gestion.Responses
{
    public class VilleResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("id_region")]
        public int? Id_Region { get; set; }

        [JsonPropertyName("id_decoupagecommercial")]
        public int? Id_DecoupageCommercial { get; set; }

        [JsonPropertyName("type_localite")]
        public string? Type_Localite { get; set; }

        [JsonPropertyName("logo")]
        public string? Logo { get; set; }
    }
}
