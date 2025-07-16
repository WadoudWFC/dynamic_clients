using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure;

public class RegisterUserRequestBody
{
    [JsonPropertyName("nom")]
    public string FirstName { get; set; } = null!;
    [JsonPropertyName("prenom")]
    public string LastName { get; set; } = null!;
    [JsonPropertyName("adress")]
    public string Address { get; set; } = null!;
    [JsonPropertyName("mail")]
    public string MailAddress { get; set; } = null!;
    [JsonPropertyName("telephone")]
    public string PhoneNumber { get; set; } = null!;
    [JsonPropertyName("id_profil")]
    public int ProfileId { get; set; }
    [JsonPropertyName("genre")]
    public bool Gender { get; set; }
}
