namespace MutipleHttpClient.Domain;

public record SanitizedUserResponse(Guid UserId, string FirstName, string LastName, bool IsActive, int AttemptsCount, bool IsPasswordUpdateRequired);
