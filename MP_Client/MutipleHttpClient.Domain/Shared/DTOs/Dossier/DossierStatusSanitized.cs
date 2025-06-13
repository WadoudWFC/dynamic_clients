using System.Text.Json.Serialization;
using MutipleHttpClient.Domain.Security;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record DossierStatusSanitized(Guid StatusId, string Label, string Status, string Code, string? Color, bool CanUpdate, bool CanUpload, bool HaveMail, string? MailName, string? MailSubject, string Action, string StatusCode)
    {
        [JsonIgnore]
        public int InternalId { get; init; }
        [JsonIgnore]
        public RoleInfo? RoleInfo { get; init; }
    }
}
