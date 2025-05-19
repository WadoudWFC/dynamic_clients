using System.Text.Json.Serialization;


namespace MultipleHtppClient.Infrastructure;

public class Aglou10001Response<T>
{
    [JsonPropertyName("codeReponse")]
    public int ResponseCode { get; set; }
    [JsonPropertyName("msg")]
    public string? Message { get; set; }
    [JsonPropertyName("data")]
    public T? Data { get; set; }
    [JsonPropertyName("NbRows")]
    public int NbRows { get; set; }
    [JsonPropertyName("TotalRows")]
    public int TotalRows { get; set; }
}
