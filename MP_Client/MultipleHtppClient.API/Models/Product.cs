using Newtonsoft.Json;

namespace MultipleHtppClient.API;

public class Product
{
    [JsonProperty("id")]
    public string? Id { get; set; }
    [JsonProperty("name")]
    public string? Name { get; set; }
    [JsonProperty("data")]
    public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
}
