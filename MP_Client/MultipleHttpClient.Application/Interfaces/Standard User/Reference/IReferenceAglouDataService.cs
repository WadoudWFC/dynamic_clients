using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public interface IReferenceAglouDataService
{
    Task<Result<IEnumerable<ActivityNatureSanitized>>> GetAllActivitiesAsync(GetAllActivitiesQuery query);
    Task<Result<IEnumerable<PackSanitized>>> GetAllPacksAsync(GetAllPackQuery query);
    Task<Result<IEnumerable<CitiesSanitized>>> GetAllCitiesAsync(GetAllCitiesQuery query);
    Task<Result<IEnumerable<ArrondissementSanitized>>> GetArrondissementsAsync(GetArrondissementQuery query);
    Task<Result<IEnumerable<TypeBienSanitized>>> GetTypeBienAsync(GetTypeBienQuery query);
    Task<Result<IEnumerable<RegionsSanitized>>> GetAllRegionsAsync(GetAllRegionQuery query);
    Task<Result<IEnumerable<PartnerTypeSanitized>>> GetPartnerTypesAsync(GetPartnerTypesQuery query);
    Task<Result<IEnumerable<DemandTypeSanitized>>> GetDemandsTypeAsync(GetDemandTypesQuery query);
    Task<Result<IEnumerable<CommercialCuttingSanitized>>> GetCommercialCuttingAsync(GetCommercialCuttingQuery query);
}
