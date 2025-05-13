using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.Models.Gestion.Requests
{
    public class ForgetPasswordRequestBody
    {
        [JsonPropertyName("mail")]
        public string Mail { get; set; }
    }
}
