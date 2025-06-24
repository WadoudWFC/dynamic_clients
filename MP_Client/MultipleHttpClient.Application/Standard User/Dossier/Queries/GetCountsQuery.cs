using MediatR;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Dossier.Queries
{
    public class GetCountsQuery : IRequest<Result<DossierCountsSanitized>>
    {
        public Guid UserId { get; set; }
        public string? RoleId { get; set; }
    }
}
