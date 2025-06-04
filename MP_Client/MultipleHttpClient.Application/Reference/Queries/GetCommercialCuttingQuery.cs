using MediatR;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Commercial_Activities.Responses;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetCommercialCuttingQuery : IRequest<Result<IEnumerable<CommercialCuttingSanitized>>> { }
