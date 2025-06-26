using MediatR;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Queries
{
    public record GetAllDossierQuery : IRequest<Result<IEnumerable<DossierAllSanitized>>>
    {
        public Guid UserId { get; set; }
        public string? RoleId { get; set; }
    }
}
