namespace MutipleHttpClient.Domain;

public record UserProfileIdResponse
{
    public Guid UserId { get; init; }
    public int ProfileId { get; init; }
    public string ProfileName { get; init; } = string.Empty;
    public string AccessLevel { get; init; } = string.Empty;
    public DateTime RetrievedAt { get; init; } = DateTime.UtcNow;
}
