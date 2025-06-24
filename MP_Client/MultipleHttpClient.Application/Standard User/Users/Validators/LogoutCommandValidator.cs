using FluentValidation;
using MultipleHttpClient.Application.Users.Commands.Logout;

namespace MultipleHttpClient.Application.Users.Validators
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Invalid User Id!");
        }
    }
}
