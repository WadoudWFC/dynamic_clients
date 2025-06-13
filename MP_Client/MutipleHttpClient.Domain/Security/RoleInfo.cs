using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Security
{
    public record RoleInfo(Guid RoleId, bool IsRegularUser)
    {
        [JsonIgnore]
        public int InternalProfileId { get; init; }
    }
}
