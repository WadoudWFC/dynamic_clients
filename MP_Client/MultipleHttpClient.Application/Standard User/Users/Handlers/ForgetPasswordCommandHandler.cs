using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Interfaces.User;
using MultipleHttpClient.Application.Users.Commands.ForgetPassword;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Users.Handlers
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, Result<SanitizedBasicResponse>>
    {
        private readonly IUserAglouService _userAglouService;
        private readonly ILogger<ForgetPasswordCommandHandler> _logger;

        // Rate limiting tracking for password reset attempts
        private static readonly Dictionary<string, List<DateTime>> _passwordResetAttempts = new();
        private static readonly object _lockObject = new object();
        private const int MAX_RESET_ATTEMPTS = 3;
        private const int RESET_WINDOW_MINUTES = 60;

        public ForgetPasswordCommandHandler(IUserAglouService userAglouService, ILogger<ForgetPasswordCommandHandler> logger)
        {
            _userAglouService = userAglouService;
            _logger = logger;
        }

        public async Task<Result<SanitizedBasicResponse>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Always return the same response regardless of email existence
            const string STANDARD_RESPONSE = "If the email address exists in our system, you will receive a password reset link shortly.";

            try
            {
                if (IsRateLimited(request.Email))
                {
                    _logger.LogWarning("Password reset rate limit exceeded for email: {0}", request.Email);
                    // Still return standard message to avoid enumeration
                    return Result<SanitizedBasicResponse>.Success(new SanitizedBasicResponse(true, STANDARD_RESPONSE));
                }
                TrackPasswordResetAttempt(request.Email);
                var result = await _userAglouService.ForgetPasswordAsync(request);
                if (result.IsSuccess)
                {
                    _logger.LogInformation("Password reset requested for valid email: {0}", request.Email);
                }
                else
                {
                    _logger.LogInformation("Password reset requested for non-existent email: {0}", request.Email);
                }
                return Result<SanitizedBasicResponse>.Success(new SanitizedBasicResponse(true, STANDARD_RESPONSE));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset for email: {0}", request.Email);

                // Even on error, return the standard message to prevent enumeration
                return Result<SanitizedBasicResponse>.Success(new SanitizedBasicResponse(true, STANDARD_RESPONSE));
            }
        }

        private bool IsRateLimited(string email)
        {
            lock (_lockObject)
            {
                if (!_passwordResetAttempts.ContainsKey(email))
                    return false;

                var attempts = _passwordResetAttempts[email];
                var cutoff = DateTime.UtcNow.AddMinutes(-RESET_WINDOW_MINUTES);

                // Remove old attempts
                attempts.RemoveAll(a => a < cutoff);

                return attempts.Count >= MAX_RESET_ATTEMPTS;
            }
        }

        private void TrackPasswordResetAttempt(string email)
        {
            lock (_lockObject)
            {
                if (!_passwordResetAttempts.ContainsKey(email))
                {
                    _passwordResetAttempts[email] = new List<DateTime>();
                }

                _passwordResetAttempts[email].Add(DateTime.UtcNow);
            }
        }
    }
}
