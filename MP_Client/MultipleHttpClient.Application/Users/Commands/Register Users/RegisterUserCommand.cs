using MediatR;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public record RegisterUserCommand(string FirstName, string LastName, string Address, string MailAddress, string PhoneNumber, bool Gender) : IRequest<Result<SanitizedBasicResponse>>;
