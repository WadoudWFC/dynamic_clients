using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Interfaces.User;
using MultipleHttpClient.Application.Users.Commands.Logout;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Users.Handlers
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<SanitizedBasicResponse>>
    {
        private readonly IUserAglouService _userAglouService;
        private readonly ILogger<LogoutCommandHandler> _logger;
        public LogoutCommandHandler(IUserAglouService userAglouService, ILogger<LogoutCommandHandler> logger)
        {
            _userAglouService = userAglouService;
            _logger = logger;
        }

        public async Task<Result<SanitizedBasicResponse>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userAglouService.LogoutAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[Logout]: {0} failed execution", nameof(LogoutCommandHandler));
                    return Result<SanitizedBasicResponse>.Failure(new Error("The LogoutCommandHandler failed", "Can't handle logout"));
                }
                _logger.LogInformation("[Logout]: Successful operation!");
                return Result<SanitizedBasicResponse>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Logout]: {0}", ex.Message);
                return Result<SanitizedBasicResponse>.Failure(new Error("The LogoutCommandHandler failed", ex.Message));
            }
        }
    }
}
