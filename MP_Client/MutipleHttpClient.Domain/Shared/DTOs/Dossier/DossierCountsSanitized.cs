using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record DossierCountsSanitized(int TotalDossier, int TotalProcessedDossier, int TotalPendingDossier, DebugInfoSanitized DebugInfo)
{
    [JsonIgnore]
    public ProfileType UserProfileId => DebugInfo.ProfileType;
}
