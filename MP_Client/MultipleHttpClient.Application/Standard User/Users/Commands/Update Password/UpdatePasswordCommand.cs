using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public record UpdatePasswordCommand(Guid UserId, string NewPassword) : IRequest<Result<SanitizedBasicResponse>>;
