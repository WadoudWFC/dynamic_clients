namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record MyDossiersDetailedSummary(int TotalDossiers, int TotalPending, int TotalProcessed, Guid UserId, string ProfileType, string AccessLevel);
}
