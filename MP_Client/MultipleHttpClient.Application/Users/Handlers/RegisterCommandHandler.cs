using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class RegisterCommandHandler : IRequestHandler<RegisterUserCommand, Result<SanitizedBasicResponse>>
{
    private readonly IUserAglouService _userAglouService;
    private readonly ILogger<RegisterCommandHandler> _logger;
    public RegisterCommandHandler(IUserAglouService userAglouService, ILogger<RegisterCommandHandler> logger)
    {
        _userAglouService = userAglouService;
        _logger = logger;
    }
    public async Task<Result<SanitizedBasicResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userAglouService.RegisterUserAsync(request);
            if (!result.IsSuccess || result.Value == null)
            {
                _logger.LogError("[Register User]: {0} failed execution", nameof(RegisterCommandHandler));
                return Result<SanitizedBasicResponse>.Failure(new Error("The RegisterCommandHandler failed", "Can't handle registration"));
            }
            _logger.LogInformation("[Register User]: Successfull operation!");
            return Result<SanitizedBasicResponse>.Success(result.Value);
        }
        catch (System.Exception ex)
        {
            _logger.LogError("[Register User]: {0}", ex.Message);
            return Result<SanitizedBasicResponse>.Failure(new Error("The RegisterCommandHandler failed", ex.Message));
        }
    }
}
