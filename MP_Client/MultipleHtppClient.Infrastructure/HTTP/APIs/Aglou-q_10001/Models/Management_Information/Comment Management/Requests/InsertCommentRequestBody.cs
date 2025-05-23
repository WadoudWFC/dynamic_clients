using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Comment_Management.Requests;

public class InsertCommentRequestBody
{
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }
    [JsonPropertyName("commentaire")]
    public string Content { get; set; }
    [JsonPropertyName("id_dossier")]
    public int DossierId { get; set; }
}
