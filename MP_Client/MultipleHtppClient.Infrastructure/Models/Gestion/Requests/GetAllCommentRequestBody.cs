using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure;

public class GetAllCommentRequestBody
{
    [JsonPropertyName("id_dossier")]
    public int DossierId { get; set; }
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }
}
