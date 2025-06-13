using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record InsertDossierOperationResult(bool IsSucess, string Message, Guid? DossierId);
}
