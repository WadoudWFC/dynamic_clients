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
        private readonly IAccountLockoutService _accountLockoutService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(IHttpUserAglou httpUserService, IJwtService jwtService, IIdMappingService idMappingService, IReferenceDataMappingService referenceDataMappingService, IAccountLockoutService accountLockoutService, ILogger<LoginCommandHandler> logger)
        {
            _httpUserService = httpUserService;
            _jwtService = jwtService;
            _idMappingService = idMappingService;
            _referenceDataMappingService = referenceDataMappingService;
            _accountLockoutService = accountLockoutService;
            _logger = logger;
        }

        public async Task<Result<SanitizedLoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("[Login]: Attempting login for {0}", request.Email);

                // SECURITY: Check if account is locked before attempting authentication
                if (await _accountLockoutService.IsAccountLockedAsync(request.Email))
                {
                    var timeRemaining = await _accountLockoutService.GetLockoutTimeRemainingAsync(request.Email);
                    _logger.LogWarning("[Login]: Account {0} is locked, {1} remaining",
                        request.Email, timeRemaining);

                    return Result<SanitizedLoginResponse>.Failure(
                        new Error("AccountLocked", $"Account is temporarily locked. Please try again in {timeRemaining?.TotalMinutes:F0} minutes."));
                }

                var loginRequest = new LoginRequestBody(request.Email, request.Password);
                var legacyResponse = await _httpUserService.LoginAsync(loginRequest);

                if (!legacyResponse.IsSuccess || legacyResponse.Data?.Data == null)
                {
                    // SECURITY: Record failed attempt for brute force protection
                    await _accountLockoutService.RecordFailedAttemptAsync(request.Email);

                    var failedAttempts = await _accountLockoutService.GetFailedAttemptsCountAsync(request.Email);
                    _logger.LogWarning("[Login]: Failed authentication for {0}, {1} failed attempts",
                        request.Email, failedAttempts);

                    // Generic error message to prevent enumeration
                    return Result<SanitizedLoginResponse>.Failure(
                        new Error("InvalidCredentials", "Invalid email or password"));
                }

                // SECURITY: Record successful login to reset failed attempts
                await _accountLockoutService.RecordSuccessfulLoginAsync(request.Email);

                var legacyUserData = legacyResponse.Data.Data;
                _logger.LogInformation("[Login]: Successful authentication for {0}, UserID: {1}",
                    request.Email, legacyUserData.UserId);

                // Continue with existing JWT generation logic...
                var userGuid = _idMappingService.GetGuidForUserId(legacyUserData.UserId);
                var profileId = legacyUserData.UserRole;

                int? commercialDivisionId = null;
                if (!string.IsNullOrEmpty(legacyUserData.DecoupageCommercialId) &&
                    int.TryParse(legacyUserData.DecoupageCommercialId, out var divisionId))
                {
                    commercialDivisionId = divisionId;
                }

                var jwtToken = await _jwtService.GenerateTokenAsync(
                    internalUserId: legacyUserData.UserId,
                    email: request.Email,
                    profileId: profileId,
                    firstName: legacyUserData.FirstName,
                    lastName: legacyUserData.LastName,
                    commercialDivisionId: commercialDivisionId,
                    parentUserId: null,
                    isActive: true
                );

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

                // SECURITY: Also record system errors as failed attempts to prevent exploitation
                await _accountLockoutService.RecordFailedAttemptAsync(request.Email);

                return Result<SanitizedLoginResponse>.Failure(
                    new Error("LoginFailed", "An error occurred during login. Please try again."));
            }
        }
    }
}
