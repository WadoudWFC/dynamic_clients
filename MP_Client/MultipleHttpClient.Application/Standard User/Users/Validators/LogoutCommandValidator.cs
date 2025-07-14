using FluentValidation;
using MultipleHttpClient.Application.Users.Commands.Logout;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Users.Validators
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage(Constants.InvalidUser);
        }
    }
}
