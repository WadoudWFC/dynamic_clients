using System.Text.Json.Serialization;
using MultipleHtppClient.Domain;
using MutipleHttpClient.Domain;


namespace MultipleHtppClient.API;

public class AglouUser
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
    [JsonPropertyName("nom")]
    public string LastName { get; set; }
    [JsonPropertyName("prenom")]
    public string FirstName { get; set; }
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
    public string Password { get; set; }
    [JsonPropertyName("date_updatepassword")]
    [JsonConverter(typeof(StringToDateTimeConverter))]
    public DateTime LastPasswordMoficationDate { get; set; }
}
