namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record MyDossiersResponse(IEnumerable<DossierSearchSanitized> Dossiers, MyDossiersSummary Summary);
}
