using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record MyDossiersSummary(int Total, Guid UserId, string ProfileType, bool HasMore, MyDossiersFilters Filters);

}
