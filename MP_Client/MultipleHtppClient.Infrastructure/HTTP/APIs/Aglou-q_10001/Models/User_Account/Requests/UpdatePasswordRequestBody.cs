using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.User_Account.Requests;
public class UpdatePasswordRequestBody
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("newPassword")]
    public string Password { get; set; } = null!;
}
