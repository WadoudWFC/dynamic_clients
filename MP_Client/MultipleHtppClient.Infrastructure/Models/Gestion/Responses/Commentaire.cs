using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure;

public class Commentaire
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("id_dossier")]
    public int? DossierId { get; set; }
    [JsonPropertyName("commentaire")]
    public string? Text { get; set; }
    [JsonPropertyName("date_created")]
    public DateTime? DateCreated { get; set; }
    [JsonPropertyName("date_updated")]
    public DateTime? DateUpdated { get; set; }
    [JsonPropertyName("date_deleted")]
    public DateTime? DateDeleted { get; set; }
    [JsonPropertyName("user_created")]
    public int? UserCreated { get; set; }
    [JsonPropertyName("user_updated")]
    public int? UserUpdated { get; set; }
    [JsonPropertyName("user_deleted")]
    public int? UserDeleted { get; set; }
    [JsonPropertyName("id_dossierNavigation")]
    public string? NavigationFolderId { get; set; }
}
