using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.User_Account.Requests;
using MultipleHttpClient.Application.Interfaces.Security;
using MultipleHttpClient.Application.Interfaces.User;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Users.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<SanitizedLoginResponse>>
    {
        private readonly IHttpUserAglou _httpUserService;
        private readonly IJwtService _jwtService;
        private readonly IIdMappingService _idMappingService;
        private readonly IReferenceDataMappingService _referenceDataMappingService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(IHttpUserAglou httpUserService, IJwtService jwtService, IIdMappingService idMappingService, IReferenceDataMappingService referenceDataMappingService, ILogger<LoginCommandHandler> logger)
        {
            _httpUserService = httpUserService;
            _jwtService = jwtService;
            _idMappingService = idMappingService;
            _referenceDataMappingService = referenceDataMappingService;
            _logger = logger;
        }

        public async Task<Result<SanitizedLoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("[Login]: Attempting login for {0}", request.Email);

                var loginRequest = new LoginRequestBody(request.Email, request.Password);
                var legacyResponse = await _httpUserService.LoginAsync(loginRequest);

                if (!legacyResponse.IsSuccess || legacyResponse.Data?.Data == null)
                {
                    _logger.LogWarning("[Login]: Legacy authentication failed for {0}", request.Email);
                    return Result<SanitizedLoginResponse>.Failure(new Error("LoginFailed", "Invalid credentials"));
                }

                var legacyUserData = legacyResponse.Data.Data;
                _logger.LogInformation("[Login]: Legacy authentication successful for {0}, UserID: {1}",
                    request.Email, legacyUserData.UserId);

                // STEP 2: Create GUID mapping for user
                var userGuid = _idMappingService.GetGuidForUserId(legacyUserData.UserId);

                // STEP 3: Extract profile information from legacy response
                // NOTE: AglouLoginResponse contains UserRole field which should be the profile ID
                var profileId = legacyUserData.UserRole; // This should be 1, 2, or 3

                // Parse commercial division if available
                int? commercialDivisionId = null;
                if (!string.IsNullOrEmpty(legacyUserData.DecoupageCommercialId) &&
                    int.TryParse(legacyUserData.DecoupageCommercialId, out var divisionId))
                {
                    commercialDivisionId = divisionId;
                }

                // STEP 4: Generate JWT using information from legacy response
                var jwtToken = await _jwtService.GenerateTokenAsync(
                    internalUserId: legacyUserData.UserId,
                    email: request.Email,
                    profileId: profileId,
                    firstName: legacyUserData.FirstName,
                    lastName: legacyUserData.LastName,
                    commercialDivisionId: commercialDivisionId,
                    parentUserId: null, // Not available in login response
                    isActive: true // Assume active if login succeeded
                );

                // STEP 5: Return sanitized response
                var response = new SanitizedLoginResponse(
                    UserId: userGuid,
                    FirstName: legacyUserData.FirstName,
                    LastName: legacyUserData.LastName,
                    IsPasswordUpdated: legacyUserData.IsPasswordUpdated,
                    BearerToken: jwtToken
                );

                _logger.LogInformation("[Login]: JWT generated successfully for user {0} with profile {1}",
                    userGuid, profileId);
                return Result<SanitizedLoginResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Login]: Exception during login for {0}", request.Email);
                return Result<SanitizedLoginResponse>.Failure(new Error("LoginFailed", "An error occurred during login"));
            }
        }
    }
}
