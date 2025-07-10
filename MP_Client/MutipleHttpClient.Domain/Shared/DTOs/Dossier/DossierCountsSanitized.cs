using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public record DossierCountsSanitized(int TotalDossier, int TotalDossierTraiter, int TotalDossierEncours, DebugInfoSanitized DebugInfo)
{
    [JsonIgnore]
    public ProfileType UserProfileId => DebugInfo.ProfileType;
}
