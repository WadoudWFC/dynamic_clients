using MediatR;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Users.Commands.ForgetPassword
{
    public record ForgetPasswordCommand(string Email) : IRequest<Result<SanitizedBasicResponse>>;
}
