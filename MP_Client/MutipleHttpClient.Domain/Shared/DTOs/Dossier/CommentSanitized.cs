using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record CommentSanitized(Guid Comment, DateTime? CreationDate, string Time, Guid DossierId, string Message, Guid UserCreatedId, UserCommentSanitized User)
{
    [JsonIgnore]
    public int InternalId { get; init; }
    [JsonIgnore]
    public int InternalDossierId { get; init; }
    [JsonIgnore]
    public int InternalUserCreatedId { get; init; }
}
