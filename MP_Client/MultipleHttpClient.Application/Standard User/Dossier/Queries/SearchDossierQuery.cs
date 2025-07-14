using System.Text.Json.Serialization;
using MediatR;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Queries
{
    public record SearchDossierQuery : IRequest<Result<IEnumerable<DossierSearchSanitized>>>
    {
        public Guid UserId { get; set; }
        public string RoleId { get; set; }
        public bool ApplyFilter { get; set; } = false;
        public string? Code { get; set; }
        public Guid? DossierStatusId { get; set; }
        public Guid? DemandTypeId { get; set; }
        public Guid? NatureActivityId { get; set; }
        public Guid? PartnerId { get; set; }
        public Guid? CommercialCuttingId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [NonNegative]
        public int Take { get; set; } = 10;
        [NonNegative]
        public int Skip { get; set; } = 0;
        public string Field { get; set; } = "code";
        public string Order { get; set; } = "desc";
        [JsonIgnore]
        public int InternalUserId { get; set; }
    }
}
