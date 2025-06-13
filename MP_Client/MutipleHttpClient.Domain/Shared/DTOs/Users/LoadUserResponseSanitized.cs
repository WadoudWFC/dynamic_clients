using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Shared.DTOs.Users
{
    public class LoadUserResponseSanitized
    {
        public Guid User { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public int Gender { get; init; }
        public string EmailAddress { get; init; }
        public string PhoneNumber { get; init; }
        [JsonPropertyName("Control")]
        public Guid Profile_Id { get; init; }
        [JsonPropertyName("Entity")]
        public string? EntityId { get; init; }
        [JsonPropertyName("DecoupageCommercial")]
        public string? DecoupageCommercialId { get; init; }
        [JsonPropertyName("ParentUser")]
        public string? ParentUserId { get; init; }
        public bool IsActive { get; init; }
        public string? ImageUrl { get; init; }
        public object? Image { get; init; }
        [JsonIgnore]
        public int InternalId { get; set; }
        [JsonIgnore]
        public int InternalProfileId { get; set; }
    }
}
