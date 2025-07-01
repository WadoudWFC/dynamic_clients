namespace MutipleHttpClient.Domain;

public record MyDossierIdsResponse(IEnumerable<DossierIdInfo> DossierIds, int Total, int Take, int Skip, bool HasMore, Guid UserId, string ProfileType);
