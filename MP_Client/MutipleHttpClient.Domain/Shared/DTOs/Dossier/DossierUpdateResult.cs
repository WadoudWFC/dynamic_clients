using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record DossierUpdateResult(bool IsSuccess, string Message)
    {
        [JsonIgnore]
        public int InternalDossierId { get; init; }
    }
}
