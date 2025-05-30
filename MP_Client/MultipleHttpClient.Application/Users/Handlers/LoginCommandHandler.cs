using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Users.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<SanitizedLoginResponse>>
    {
        private readonly IUserAglouService _userAglouService;
        private readonly ILogger<LoginCommandHandler> _logger;
        public LoginCommandHandler(IUserAglouService userAglouService, ILogger<LoginCommandHandler> logger)
        {
            _userAglouService = userAglouService;
            _logger = logger;
        }

        public async Task<Result<SanitizedLoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userAglouService.LoginAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[Login]: {0} failed execution!", nameof(LoginCommandHandler));
                    return Result<SanitizedLoginResponse>.Failure(new Error("The LoginCommandHandler failed", "Can't handle login"));
                }
                _logger.LogInformation("[Login]: Successful operation!");
                return Result<SanitizedLoginResponse>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Login]: {0}", ex.Message);
                return Result<SanitizedLoginResponse>.Failure(new Error("The LoginCommandHandler failed", ex.Message));
            }
        }
    }
}
