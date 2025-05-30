using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Users.Commands.Can_Try_Login;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Users.Handlers
{
    public class CanTryLoginCommandHandler : IRequestHandler<CanTryLoginCommand, Result<SanitizedUserResponse>>
    {
        private readonly IUserAglouService _userAglouService;
        private readonly ILogger<CanTryLoginCommandHandler> _logger;
        public CanTryLoginCommandHandler(IUserAglouService userAglouService, ILogger<CanTryLoginCommandHandler> logger)
        {
            _userAglouService = userAglouService;
            _logger = logger;
        }

        public async Task<Result<SanitizedUserResponse>> Handle(CanTryLoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userAglouService.CanTryLoginAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[CanTryLogin]: {0} failed execution", nameof(CanTryLoginCommandHandler));
                    return Result<SanitizedUserResponse>.Failure(new Error("The CanTryLoginCommandHandler failed", "Can't handle login"));
                }
                _logger.LogInformation("[CanTryLogin]: Successful operation!");
                return Result<SanitizedUserResponse>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Login]: {0}", ex.Message);
                return Result<SanitizedUserResponse>.Failure(new Error("The CanTryLoginCommandHandler failed", ex.Message));
            }
        }
    }
}
