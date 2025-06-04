using System.Collections.Concurrent;

namespace MutipleHttpClient.Domain;

public class ReferenceDataMappingService : IReferenceDataMappingService
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, int>> _guidToIdMap = new ConcurrentDictionary<string, ConcurrentDictionary<Guid, int>>();
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<int, Guid>> _idToGuidMap = new ConcurrentDictionary<string, ConcurrentDictionary<int, Guid>>();

    public Guid GetOrCreateGuidForReferenceId(int referenceId, string entityType)
    {
        var idMap = _idToGuidMap.GetOrAdd(entityType, _ => new ConcurrentDictionary<int, Guid>());
        if (idMap.TryGetValue(referenceId, out var existingGuid))
        {
            return existingGuid;
        }
        var newGuid = Guid.NewGuid();
        idMap[referenceId] = newGuid;
        _guidToIdMap.GetOrAdd(entityType, _ => new ConcurrentDictionary<Guid, int>())[newGuid] = referenceId;
        return newGuid;
    }

    public int? GetReferenceIdForGuid(Guid guid, string entityType)
    {
        if (_guidToIdMap.TryGetValue(entityType, out var typeMap) && typeMap.TryGetValue(guid, out var referenceId)) return referenceId;
        return null;
    }

    public void RemoveReferenceMapping(Guid guid, string entityType)
    {
        if (_guidToIdMap.TryGetValue(entityType, out var guidMap) && guidMap.TryRemove(guid, out var referenceId))
        {
            if (_idToGuidMap.TryGetValue(entityType, out var idMap))
            {
                idMap.TryRemove(referenceId, out _);
            }
        }
    }
}
