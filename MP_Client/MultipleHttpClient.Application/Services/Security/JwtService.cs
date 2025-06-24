using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MultipleHttpClient.Application.Interfaces.Security;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Services.Security
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IIdMappingService _idMappingService;
        private readonly IReferenceDataMappingService _referenceDataMappingService;

        public JwtService(IOptions<JwtSettings> jwtSettings,
                         IIdMappingService idMappingService,
                         IReferenceDataMappingService referenceDataMappingService)
        {
            _jwtSettings = jwtSettings.Value;
            _idMappingService = idMappingService;
            _referenceDataMappingService = referenceDataMappingService;
        }

        /// <summary>
        /// Generate JWT token using provided user details (no circular dependency)
        /// </summary>
        public async Task<string> GenerateTokenAsync(int internalUserId, string email, int profileId, string firstName, string lastName, int? commercialDivisionId = null, int? parentUserId = null, bool isActive = true)
        {
            try
            {
                // Get user GUID from internal ID
                var userGuid = _idMappingService.GetGuidForUserId(internalUserId);

                // Convert profile ID to GUID for external use
                var profileGuid = _referenceDataMappingService.GetOrCreateGuidForReferenceId(profileId, Constants.Profile);

                var claims = new List<Claim>
            {
                // External identifiers (visible to client)
                new Claim("user_id", userGuid.ToString()),
                new Claim("profile_id", profileGuid.ToString()),
                new Claim("email", email),
                new Claim("first_name", firstName),
                new Claim("last_name", lastName),
                
                // Internal identifiers (hidden from client, used for authorization)
                new Claim("internal_user_id", internalUserId.ToString()),
                new Claim("internal_profile_id", profileId.ToString()),
                
                // Role-based claims for easy authorization
                new Claim("role", GetRoleName(profileId)),
                new Claim("is_admin", (profileId == 1).ToString()),
                new Claim("is_regional_admin", (profileId == 2).ToString()),
                new Claim("is_standard_user", (profileId == 3).ToString()),
                
                // Additional context for authorization
                new Claim("is_active", isActive.ToString()),
                new Claim("commercial_division_id", commercialDivisionId?.ToString() ?? ""),
                new Claim("parent_user_id", parentUserId?.ToString() ?? ""),
                
                // Standard JWT claims
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

                var key = new SymmetricSecurityKey(Convert.FromBase64String(_jwtSettings.Secret));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirayMinutes),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to generate JWT token for user {internalUserId}: {ex.Message}", ex);
            }
        }

        private static string GetRoleName(int profileId) => profileId switch
        {
            1 => "Admin",
            2 => "RegionalAdmin",
            3 => "StandardUser",
            _ => "Unknown"
        };

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Convert.FromBase64String(_jwtSettings.Secret);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }

        public bool IsTokenValid(string token)
        {
            return ValidateToken(token) != null;
        }
    }
}
