using MediatR;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Users.Commands.Logout
{
    public record LogoutCommand(Guid UserId) : IRequest<Result<SanitizedBasicResponse>>;
}
