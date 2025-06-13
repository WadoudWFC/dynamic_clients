using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record UserCommentSanitized(Guid User, string? Image, string Align, string FullName)
{
    [JsonIgnore]
    public int InternalId { get; init; }
}
