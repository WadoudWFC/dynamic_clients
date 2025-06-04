using MultipleHtppClient.Infrastructure;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Commercial_Activities.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Demand_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Gepgraphical_Information.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Partner_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Property_Information.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;
using MultipleHtppClient.Infrastructure.HTTP.REST;
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
