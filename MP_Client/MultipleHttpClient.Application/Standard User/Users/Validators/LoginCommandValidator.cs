using FluentValidation;

namespace MultipleHttpClient.Application.Users.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required!")
                                .EmailAddress().WithMessage("Invalid Email format!")
                                .MaximumLength(80).WithMessage("Email must be <= 80 characters");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required!")
                                .MinimumLength(8).WithMessage("Password must be >= 8 characters")
                                .MaximumLength(24).WithMessage("Password must be <= 24 character");
    }
}
