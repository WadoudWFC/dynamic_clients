using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Users.Commands.ForgetPassword;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Users.Handlers
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, Result<SanitizedBasicResponse>>
    {
        private readonly IUserAglouService _userAglouService;
        private readonly ILogger<ForgetPasswordCommand> _logger;
        public ForgetPasswordCommandHandler(IUserAglouService userAglouService, ILogger<ForgetPasswordCommand> logger)
        {
            _userAglouService = userAglouService;
            _logger = logger;
        }

        public async Task<Result<SanitizedBasicResponse>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userAglouService.ForgetPasswordAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[Forget Password]: {0} failed execution", nameof(ForgetPasswordCommandHandler));
                    return Result<SanitizedBasicResponse>.Failure(new Error("The ForgetPasswordCommandHandler failed", "Can't handle forget password"));
                }
                _logger.LogInformation("[Forget Password]: Successful operation!");
                return Result<SanitizedBasicResponse>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Forget Password]: {0}", ex.Message);
                return Result<SanitizedBasicResponse>.Failure(new Error("The ForgetPasswordCommandHandler failed", ex.Message));
            }
        }
    }
}
