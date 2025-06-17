using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record PartnerSanitized(Guid PartnerId, Guid? PartnerTypeId, Guid? PersonTypeId, Guid? CityId, Guid? RegionId,
                                    string? Code, string? LastName, string? FirstName, string? Email, string? Phone,
                                    string? ICE, string? CompanyName, string? FiscalIdentification, int LegalForm,
                                    int TaxRegime, int AgentMode)
    {
        [JsonIgnore]
        public int InternalId { get; init; }
        [JsonIgnore]
        public int? InternalPartnerTypeId { get; init; }
        [JsonIgnore]
        public int? InternalPersonTypeId { get; init; }
        [JsonIgnore]
        public int? InternalCityId { get; init; }
        [JsonIgnore]
        public int? InternalRegionId { get; init; }
    };
}
