using MediatR;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Users;

namespace MultipleHttpClient.Application.Users.Commands.LoadUser
{
    public class LoadUserCommand : IRequest<Result<LoadUserResponseSanitized>>
    {
        public Guid UserId { get; set; }
    }
}
