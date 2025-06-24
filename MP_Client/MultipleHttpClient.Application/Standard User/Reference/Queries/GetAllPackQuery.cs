using MediatR;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetAllPackQuery : IRequest<Result<IEnumerable<PackSanitized>>> { }
