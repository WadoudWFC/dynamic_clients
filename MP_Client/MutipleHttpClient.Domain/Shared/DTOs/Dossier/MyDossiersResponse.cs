namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record MyDossiersResponse(IEnumerable<DossierSearchSanitized> Dossiers, int Total, int Take, int Skip, bool HasMore, Guid UserId, string ProfileType);
}
