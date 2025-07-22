using System.Text.Json.Serialization;
using MutipleHttpClient.Domain.Converters.Types;


namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Models.User_Account.Responses;

public class AglouUser
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = null!;
    [JsonPropertyName("nom")]
    public string LastName { get; set; } = null!;
    [JsonPropertyName("prenom")]
    public string FirstName { get; set; } = null!;
    [JsonPropertyName("date_tent")]
    [JsonConverter(typeof(StringToDateTimeConverter))]
    public DateTime ConnectionTryDate { get; set; }
    [JsonPropertyName("isactive")]
    [JsonConverter(typeof(StringToBoolConverter))]
    public bool IsUserActive { get; set; }
    [JsonPropertyName("tentativecount")]
    [JsonConverter(typeof(StringToIntConverter))]
    public int TryingCounter { get; set; }
    [JsonPropertyName("isupdatepassword")]
    [JsonConverter(typeof(StringToBoolConverter))]
    public bool IsPasswordUpdated { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
    [JsonPropertyName("date_updatepassword")]
    [JsonConverter(typeof(StringToDateTimeConverter))]
    public DateTime LastPasswordMoficationDate { get; set; }
}
