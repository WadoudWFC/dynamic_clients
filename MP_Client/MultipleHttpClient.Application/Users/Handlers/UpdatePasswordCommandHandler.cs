using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Interfaces.User;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Users.Handlers
{
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, Result<SanitizedBasicResponse>>
    {
        private readonly IUserAglouService _userAglouService;
        private readonly ILogger<UpdatePasswordCommand> _logger;
        public UpdatePasswordCommandHandler(IUserAglouService userAglouService, ILogger<UpdatePasswordCommand> logger)
        {
            _userAglouService = userAglouService;
            _logger = logger;
        }

        public async Task<Result<SanitizedBasicResponse>> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userAglouService.UpdatePasswordAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[Update Password]: {0} failed execution", nameof(UpdatePasswordCommandHandler));
                    return Result<SanitizedBasicResponse>.Failure(new Error("The UpdatePasswordCommandHandler failed", "Can't handle update password!"));
                }
                _logger.LogInformation("[Update Password]: Successful operation!");
                return Result<SanitizedBasicResponse>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Update Password]: {0}", ex.Message);
                return Result<SanitizedBasicResponse>.Failure(new Error("The UpdatePasswordCommandHandler failed", ex.Message));
            }
        }
    }
}
