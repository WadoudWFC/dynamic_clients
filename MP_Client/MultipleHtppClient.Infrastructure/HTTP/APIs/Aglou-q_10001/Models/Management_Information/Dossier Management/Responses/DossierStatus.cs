using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Responses
{
    public class DossierStatus
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;
        [JsonPropertyName("statut")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;
        [JsonPropertyName("color")]
        public string? Color { get; set; }
        [JsonPropertyName("canupdate")]
        public bool CanUpdate { get; set; }
        [JsonPropertyName("canuploade")]
        public bool CanUpload { get; set; }
        [JsonPropertyName("havemail")]
        public bool HaveMail { get; set; }
        [JsonPropertyName("mailname")]
        public string? MailName { get; set; }
        [JsonPropertyName("mailSubject")]
        public string? MailSubject { get; set; }
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;
        [JsonPropertyName("statuscode")]
        public string StatusCode { get; set; } = string.Empty;
    }
}
