using MediatR;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetPartnerTypesQuery : IRequest<Result<IEnumerable<PartnerTypeSanitized>>> { }
