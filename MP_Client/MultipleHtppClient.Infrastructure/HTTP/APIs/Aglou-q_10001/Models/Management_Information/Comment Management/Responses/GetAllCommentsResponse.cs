using System.Text.Json.Serialization;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Comment_Management.Responses;

public class GetAllCommentsResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("date_created")]
    public string? CreationDate { get; set; }
    [JsonPropertyName("time")]
    public string? Time { get; set; }
    [JsonPropertyName("id_dossier")]
    public int DossierId { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("user_created")]
    public int UserCreatedId { get; set; }
    [JsonPropertyName("user")]
    public UserCommentResponse User { get; set; }
}
