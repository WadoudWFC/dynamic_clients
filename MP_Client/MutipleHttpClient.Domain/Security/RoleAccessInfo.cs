using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutipleHttpClient.Domain.Security
{
    public record RoleAccessInfo(Guid RoleId, bool IsRegularUser, DateTime Expiration);
}
