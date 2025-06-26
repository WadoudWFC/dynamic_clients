namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record MyDossiersStatistics(IEnumerable<StatusGroup> ByStatus,IEnumerable<TypeGroup> ByType,IEnumerable<DossierSearchSanitized> RecentDossiers,IEnumerable<DossierSearchSanitized> OldestDossiers);
}
