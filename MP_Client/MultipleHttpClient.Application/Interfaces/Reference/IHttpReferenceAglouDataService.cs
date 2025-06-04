using MultipleHtppClient.Infrastructure;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Commercial_Activities.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Demand_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Gepgraphical_Information.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Partner_Management.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Property_Information.Responses;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Supplements.Responses;
using MultipleHtppClient.Infrastructure.HTTP.REST;

namespace MultipleHttpClient.Application.Interfaces.Reference;

public interface IHttpReferenceAglouDataService
{
    Task<ApiResponse<Aglou10001Response<IEnumerable<ActivityNatureResponse>>>> GetAllActivitiesAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<PackResponse>>>> GetAllPacksAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<VilleResponse>>>> GetAllCitiesAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<ArrondissementResponse>>>> GetArrondissementsAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<TypeBienResponse>>>> GetTypeBienAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<RegionResponse>>>> GetAllRegionsAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<PartnersType>>>> GetPartnerTypesAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<DemandType>>>> GetDemandsTypeAsync();
    Task<ApiResponse<Aglou10001Response<IEnumerable<CommercialCutting>>>> GetCommercialCuttingAsync();
}