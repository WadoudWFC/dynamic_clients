namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record StatusGroup(string Status, int Count, IEnumerable<DossierSearchSanitized> Dossiers);

}
