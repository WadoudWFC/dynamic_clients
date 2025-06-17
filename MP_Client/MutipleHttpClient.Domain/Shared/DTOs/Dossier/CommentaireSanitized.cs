using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record CommentaireSanitized(Guid CommentId, Guid DossierId, string? Text, DateTime? DateCreated, Guid? UserCreatedId)
    {
        [JsonIgnore]
        public int InternalId { get; init; }
        [JsonIgnore]
        public int InternalDossierId { get; init; }
        [JsonIgnore]
        public int? InternalUserCreatedId { get; init; }
    };
}
