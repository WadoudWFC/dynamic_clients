using MediatR;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public record GetMyDossierIdsQuery : IRequest<Result<MyDossierIdsResponse>>
{
    public Guid UserId { get; init; }
    public string RoleId { get; init; } = string.Empty;
    public int Take { get; init; } = 100;
    public int Skip { get; init; } = 0;
}