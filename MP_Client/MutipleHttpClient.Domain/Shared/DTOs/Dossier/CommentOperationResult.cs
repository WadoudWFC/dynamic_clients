using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record CommentOperationResult(bool IsSuccess, string Message, Guid? CommentId)
    {
        [JsonIgnore]
        public int InternalCommentId { get; init; }
    }
}
