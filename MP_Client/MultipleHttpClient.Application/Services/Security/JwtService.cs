using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<JwtService> _logger;

        public JwtService(IOptions<JwtSettings> jwtSettings, IIdMappingService idMappingService, IReferenceDataMappingService referenceDataMappingService, ILogger<JwtService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _idMappingService = idMappingService;
            _referenceDataMappingService = referenceDataMappingService;
            _logger = logger;
        }

        public async Task<string> GenerateTokenAsync(int internalUserId, string email, int profileId, string firstName, string lastName, int? commercialDivisionId = null, int? parentUserId = null, bool isActive = true)
        {
            try
            {
                if (!IsValidProfileId(profileId))
                {
                    _logger.LogError("SECURITY: Invalid profile ID {0} for user {1}", profileId, email);
                    throw new SecurityException($"Invalid profile ID: {profileId}");
                }

                // Get user GUID from internal ID
                var userGuid = _idMappingService.GetGuidForUserId(internalUserId);
                var profileGuid = _referenceDataMappingService.GetOrCreateGuidForReferenceId(profileId, Constants.Profile);

                // SECURITY: Create commercial division GUID if exists
                Guid? commercialDivisionGuid = null;
                if (commercialDivisionId.HasValue)
                {
                    commercialDivisionGuid = _referenceDataMappingService.GetOrCreateGuidForReferenceId(commercialDivisionId.Value, Constants.CommercialDivision);
                }

                // SECURITY: Create parent user GUID if exists
                Guid? parentUserGuid = null;
                if (parentUserId.HasValue)
                {
                    parentUserGuid = _idMappingService.GetGuidForUserId(parentUserId.Value);
                }

                var claims = new List<Claim>
        {
            // SECURE: Only external GUIDs (no internal IDs)
            new Claim("user_id", userGuid.ToString()),
            new Claim("profile_id", profileGuid.ToString()),
            new Claim("email", email),
            new Claim("first_name", firstName),
            new Claim("last_name", lastName),
            
            // SECURE: Convert internal IDs to GUIDs
            new Claim("commercial_division_id", commercialDivisionGuid?.ToString() ?? ""),
            new Claim("parent_user_id", parentUserGuid?.ToString() ?? ""),
            
            // Role-based claims for authorization
            new Claim("role", GetRoleName(profileId)),
            new Claim("is_admin", (profileId == 1).ToString()),
            new Claim("is_regional_admin", (profileId == 2).ToString()),
            new Claim("is_standard_user", (profileId == 3).ToString()),
            
            // Additional context
            new Claim("is_active", isActive.ToString()),
            
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

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                _logger.LogInformation("JWT generated for user {0} with profile {1}",
                    email, profileId);
                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate JWT token for user {0} with profile {1}", email, profileId);
                throw new InvalidOperationException($"Failed to generate JWT token for user {internalUserId}: {ex.Message}", ex);
            }
        }
        private static string GetApiType(int profileId) => profileId switch
        {
            1 => "admin",
            2 => "regional_admin",
            3 => "standard",
            _ => "restricted"
        };


        private static string GetAccessLevel(int profileId) => profileId switch
        {
            1 => "full",
            2 => "regional",
            3 => "limited",
            _ => "none"
        };


        private static string GetRoleName(int profileId) => profileId switch
        {
            1 => "Admin",
            2 => "RegionalAdmin",
            3 => "StandardUser",
            _ => "Unknown"
        };

        private static bool IsValidProfileId(int profileId)
        {
            var validProfiles = new[] { 1, 2, 3, 13, 14, 15, 16 };
            return validProfiles.Contains(profileId);
        }

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
            catch (Exception ex)
            {
                _logger.LogWarning("Token validation failed: {0}", ex.Message);
                return null;
            }
        }

        public bool IsTokenValid(string token)
        {
            return ValidateToken(token) != null;
        }
    }

    // Custom exception for security violations
    public class SecurityException : Exception
    {
        public SecurityException(string message) : base(message) { }
        public SecurityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
