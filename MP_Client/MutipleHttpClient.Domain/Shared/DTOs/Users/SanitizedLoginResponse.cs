namespace MutipleHttpClient.Domain;

public record SanitizedLoginResponse(Guid UserId, string FirstName, string LastName, bool IsPasswordUpdated, string BearerToken);
