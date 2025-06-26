using MediatR;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Standard_User.Dossier.Queries
{
    public record GetMyDossiersQuery : IRequest<Result<IEnumerable<DossierSearchSanitized>>>
    {
        public Guid UserId { get; init; }
        public string RoleId { get; init; } = string.Empty;
        public int? Take { get; init; } = 10; 
        public int? Skip { get; init; } = 0;
        public string? Order { get; init; } = "desc";
        public string? Field { get; init; } = "date_created";
        internal int? InternalUserId { get; set; }
        internal int? InternalProfileId { get; set; }
    }
}
