using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;

public class UserCommentResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("image")]
    public string? Image { get; set; }
    [JsonPropertyName("align")]
    public string? Align { get; set; }
    [JsonPropertyName("fullnam")]
    public string? FullName { get; set; }
}
