namespace MutipleHttpClient.Domain;

public interface IIdMappingService
{
    Guid GetGuidForUserId(int userId);
    int? GetUserIdForGuid(Guid guid);
    void RemoveMapping(Guid guid);
}
