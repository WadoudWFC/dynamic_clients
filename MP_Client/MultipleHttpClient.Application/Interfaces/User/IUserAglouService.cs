using MultipleHttpClient.Application.Users.Commands.Can_Try_Login;
using MultipleHttpClient.Application.Users.Commands.ForgetPassword;
using MultipleHttpClient.Application.Users.Commands.Logout;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Interfaces.User;

public interface IUserAglouService
{
    Task<Result<SanitizedLoginResponse>> LoginAsync(LoginCommand command);
    Task<Result<SanitizedUserResponse>> CanTryLoginAsync(CanTryLoginCommand command);
    Task<Result<SanitizedBasicResponse>> LogoutAsync(LogoutCommand command);
    Task<Result<SanitizedBasicResponse>> ForgetPasswordAsync(ForgetPasswordCommand command);
    Task<Result<SanitizedBasicResponse>> UpdatePasswordAsync(UpdatePasswordCommand command);
    Task<Result<SanitizedBasicResponse>> RegisterUserAsync(RegisterUserCommand command);
}
