using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Interfaces.User;
using MultipleHttpClient.Application.Users.Commands.LoadUser;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Users;

namespace MultipleHttpClient.Application.Users.Handlers
{
    public class GetUserCommandHandler : IRequestHandler<LoadUserCommand, Result<LoadUserResponseSanitized>>
    {
        private readonly IUserAglouService _userAglouService;
        private readonly ILogger<GetUserCommandHandler> _logger;

        public GetUserCommandHandler(IUserAglouService userAglouService, ILogger<GetUserCommandHandler> logger)
        {
            _userAglouService = userAglouService;
            _logger = logger;
        }

        public async Task<Result<LoadUserResponseSanitized>> Handle(LoadUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userAglouService.GetUserByIdAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[GetUserCommand]: Failed to load user {UserId}", request.UserId);
                    return Result<LoadUserResponseSanitized>.Failure(new Error("GetUserFailed", "Unable to load user details"));
                }

                _logger.LogInformation("[GetUserCommand]: Successfully loaded user {UserId}", request.UserId);
                return Result<LoadUserResponseSanitized>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GetUserCommand]: Exception loading user {UserId}", request.UserId);
                return Result<LoadUserResponseSanitized>.Failure(new Error("GetUserFailed", ex.Message));
            }
        }
    }
}
