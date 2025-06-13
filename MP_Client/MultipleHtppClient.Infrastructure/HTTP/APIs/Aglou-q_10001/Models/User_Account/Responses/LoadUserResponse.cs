using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.User_Account.Responses
{
    public class LoadUserResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nom")]
        public string LastName { get; set; }
        [JsonPropertyName("prenom")]
        public string FirstName { get; set; }
        [JsonPropertyName("genre")]
        public int Gender { get; set; }
        [JsonPropertyName("mail")]
        public string EmailAddress { get; set; }
        [JsonPropertyName("telephone")]
        public string PhoneNumber { get; set; }
        [JsonPropertyName("id_profil")]
        public int ProfileId { get; set; }
        [JsonPropertyName("id_entity")]
        public string? EntityId { get; set; }
        [JsonPropertyName("id_decoupagecommercial")]
        public string? DecoupageCommercialId { get; set; }
        [JsonPropertyName("id_user_pere")]
        public string? ParentUserId { get; set; }
        [JsonPropertyName("isactive")]
        public bool IsActive { get; set; }
        [JsonPropertyName("imageUrl")]
        public string? ImageUrl { get; set; }
        [JsonPropertyName("image")]
        public object? Image { get; set; }
    }
}
