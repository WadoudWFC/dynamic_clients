using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure;

public class Product
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("data")]
    public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
}
