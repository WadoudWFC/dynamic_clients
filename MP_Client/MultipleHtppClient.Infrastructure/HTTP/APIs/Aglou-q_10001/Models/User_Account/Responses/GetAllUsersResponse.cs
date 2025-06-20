using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure;

public class GetAllUsersResponse
{
    [JsonPropertyName("id")]
    public int UserId { get; set; }
    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = null!;
}

