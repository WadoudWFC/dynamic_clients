using MediatR;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Queries
{
    public record LoadDossierQuery(Guid DossierId) : IRequest<Result<LoadDossierResponseSanitized>>;
}
