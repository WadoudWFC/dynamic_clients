using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultipleHtppClient.Infrastructure.Models.Gestion.Responses
{
    public class RegionResponse
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("label")]
        public string? Label { get; set; }
        [JsonPropertyName("logo")]
        public string? Logo { get; set; }
        [JsonPropertyName("map")]
        public string? Map { get; set; }
    }
}
