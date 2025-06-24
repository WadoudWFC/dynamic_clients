using System.Security.Claims;

namespace MultipleHttpClient.Application.Interfaces.Security
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(int internalUserId, string email, int profileId, string firstName, string lastName, int? commercialDivisionId = null, int? parentUserId = null, bool isActive = true);
        ClaimsPrincipal? ValidateToken(string token);
        bool IsTokenValid(string token);
    }
}
