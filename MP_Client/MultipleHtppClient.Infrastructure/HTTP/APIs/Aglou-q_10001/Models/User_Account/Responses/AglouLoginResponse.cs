using System.Text.Json.Serialization;
using MutipleHttpClient.Domain.Converters.Types;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Models.User_Account.Responses;

public class AglouLoginResponse
{
    [JsonPropertyName("XKestrel")]
    public string BearerKey { get; set; }
    [JsonPropertyName("user_id")]
    [JsonConverter(typeof(StringToIntConverter))]
    public int UserId { get; set; }
    [JsonPropertyName("id_typepartenaire")]
    public string? PartnershipType { get; set; }
    [JsonPropertyName("nom")]
    public string LastName { get; set; }
    [JsonPropertyName("prenom")]
    public string FirstName { get; set; }
    [JsonPropertyName("isupdatepassword")]
    [JsonConverter(typeof(StringToBoolConverter))]
    public bool IsPasswordUpdated { get; set; }
    [JsonPropertyName("user_role")]
    [JsonConverter(typeof(StringToIntConverter))]
    public int UserRole { get; set; }
    [JsonPropertyName("role_label")]
    public string RoleLabel { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("id_decoupagecommercial")]
    public string? DecoupageCommercialId { get; set; }
    [JsonPropertyName("image")]
    public string? Image { get; set; }
    [JsonPropertyName("isotp")]
    [JsonConverter(typeof(StringToBoolConverter))]
    public bool IsOtp { get; set; }
}
