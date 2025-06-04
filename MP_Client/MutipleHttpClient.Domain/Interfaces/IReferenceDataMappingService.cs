namespace MutipleHttpClient.Domain;

public interface IReferenceDataMappingService
{
    Guid GetOrCreateGuidForReferenceId(int referenceId, string entityType);
    int? GetReferenceIdForGuid(Guid guid, string entityType);
    void RemoveReferenceMapping(Guid guid, string entityType);
}
