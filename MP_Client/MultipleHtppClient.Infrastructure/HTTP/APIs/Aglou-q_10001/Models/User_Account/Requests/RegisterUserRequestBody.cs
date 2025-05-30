using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure;

public class RegisterUserRequestBody
{
    [JsonPropertyName("nom")]
    public string FirstName { get; set; }
    [JsonPropertyName("prenom")]
    public string LastName { get; set; }
    [JsonPropertyName("adress")]
    public string Address { get; set; }
    [JsonPropertyName("mail")]
    public string MailAddress { get; set; }
    [JsonPropertyName("telephone")]
    public string PhoneNumber { get; set; }
    [JsonPropertyName("id_profil")]
    public int ProfileId { get; set; }
    [JsonPropertyName("genre")]
    public bool Gender { get; set; }
}
