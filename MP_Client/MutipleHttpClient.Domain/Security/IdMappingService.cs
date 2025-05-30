using System.Collections.Concurrent;

namespace MutipleHttpClient.Domain;

public class IdMappingService : IIdMappingService
{
    private readonly ConcurrentDictionary<Guid, int> _guidToIdMap = new ConcurrentDictionary<Guid, int>();
    private readonly ConcurrentDictionary<int, Guid> _idToGuidMap = new ConcurrentDictionary<int, Guid>();
    public Guid GetGuidForUserId(int userId)
    {
        if (_idToGuidMap.TryGetValue(userId, out var guid)) return guid;
        guid = Guid.NewGuid();
        _guidToIdMap[guid] = userId;
        _idToGuidMap[userId] = guid;
        return guid;
    }

    public int? GetUserIdForGuid(Guid guid)
    {
        return _guidToIdMap.TryGetValue(guid, out var userId) ? userId : null;
    }

    public void RemoveMapping(Guid guid)
    {
        if (_guidToIdMap.TryRemove(guid, out var userId))
            _idToGuidMap.TryRemove(userId, out _);
    }
}
