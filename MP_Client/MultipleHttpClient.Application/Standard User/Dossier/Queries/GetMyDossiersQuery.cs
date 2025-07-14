using MediatR;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Standard_User.Dossier.Queries
{
    public record GetMyDossiersQuery : IRequest<Result<MyDossiersResponse>>
    {
        public Guid UserId { get; init; }
        public string RoleId { get; init; } = string.Empty;
        [NonNegative]
        public int Take { get; init; } = 50;
        [NonNegative]
        public int Skip { get; init; } = 0;
    }
}
