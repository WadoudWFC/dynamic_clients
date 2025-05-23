using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.User_Account.Requests
{
    public class LogoutRequestBody
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;
    }
}
