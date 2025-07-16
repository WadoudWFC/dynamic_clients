using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.User_Account.Requests
{
    public class ForgetPasswordRequestBody
    {
        [JsonPropertyName("mail")]
        public string Mail { get; set; } = null!;
    }
}
