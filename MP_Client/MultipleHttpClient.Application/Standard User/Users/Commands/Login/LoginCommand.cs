using MediatR;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public record class LoginCommand(string Email, string Password) : IRequest<Result<SanitizedLoginResponse>>;
