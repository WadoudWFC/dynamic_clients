using MediatR;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Users.Commands.Can_Try_Login
{
    public record CanTryLoginCommand(string Email) : IRequest<Result<SanitizedUserResponse>>;
}
