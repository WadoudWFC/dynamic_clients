using FluentValidation;
using MultipleHttpClient.Application.Users.Commands.Can_Try_Login;

namespace MultipleHttpClient.Application.Users.Validators
{
    public class CanTryLoginCommandValidator : AbstractValidator<CanTryLoginCommand>
    {
        public CanTryLoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required!")
                                    .EmailAddress().WithMessage("Invalid Email address!")
                                    .MaximumLength(80).WithMessage("Email must be <= 80 characters");
        }
    }
}
