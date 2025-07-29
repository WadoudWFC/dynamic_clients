using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Command;
using MultipleHttpClient.Application.Interfaces.User;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, Result<SanitizedBasicResponse>>
    {
        private readonly IUserAglouService _userAglouService;
        private readonly ILogger<UpdatePasswordCommandHandler> _logger;

        public UpdatePasswordCommandHandler(IUserAglouService userAglouService, ILogger<UpdatePasswordCommandHandler> logger)
        {
            _userAglouService = userAglouService;
            _logger = logger;
        }

        public async Task<Result<SanitizedBasicResponse>> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // SECURITY: Additional validation - ensure UserId is not Guid.Empty
                if (request.UserId == Guid.Empty)
                {
                    _logger.LogError("SECURITY: Password update attempted with empty UserId");
                    return Result<SanitizedBasicResponse>.Failure(
                        new Error("SecurityError", "Invalid user identification"));
                }

                // SECURITY: Log the password update attempt
                _logger.LogInformation("Processing password update for user {UserId}", request.UserId);

                var result = await _userAglouService.UpdatePasswordAsync(request);

                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("Password update failed for user {UserId}: {Error}",
                        request.UserId, result.Error?.Message);
                    return Result<SanitizedBasicResponse>.Failure(
                        new Error("UpdatePasswordFailed", "Password update failed"));
                }

                _logger.LogInformation("Password update completed successfully for user {UserId}", request.UserId);
                return Result<SanitizedBasicResponse>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SECURITY: Exception during password update for user {UserId}", request.UserId);
                return Result<SanitizedBasicResponse>.Failure(
                    new Error("UpdatePasswordFailed", "An error occurred during password update"));
            }
        }
    }
}
