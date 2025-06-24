using MediatR;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Gepgraphical_Information.Responses;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetAllCitiesQuery : IRequest<Result<IEnumerable<CitiesSanitized>>> { }
