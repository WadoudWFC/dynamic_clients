using MediatR;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Queries
{
    public class GetDossierStatusQuery : IRequest<Result<IEnumerable<DossierStatusSanitized>>>
    {
        public Guid UserId { get; set; }
        public Guid DossierId { get; set; }
    }
}
