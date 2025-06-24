using MediatR;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetDemandTypesQuery : IRequest<Result<IEnumerable<DemandTypeSanitized>>> { }
