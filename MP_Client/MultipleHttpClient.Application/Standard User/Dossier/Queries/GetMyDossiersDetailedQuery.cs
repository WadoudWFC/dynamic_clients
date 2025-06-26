using MediatR;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Standard_User.Dossier.Queries
{
    public record GetMyDossiersDetailedQuery : IRequest<Result<MyDossiersDetailedResponse>>
    {
        public Guid UserId { get; init; }
        public string RoleId { get; init; } = string.Empty;
    }
}
