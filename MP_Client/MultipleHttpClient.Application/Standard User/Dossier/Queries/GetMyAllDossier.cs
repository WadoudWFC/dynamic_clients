using MediatR;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Standard_User.Dossier.Queries
{
    public class GetMyAllDossier : IRequest<Result<IEnumerable<DossierSearchSanitized>>>
    {
    }
}
