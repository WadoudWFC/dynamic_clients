using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure;

public class UpdatePasswordRequestBody
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("newPassword")]
    public string Password { get; set; } = null!;
}
