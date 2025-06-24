using FluentValidation;

namespace MultipleHttpClient.Application;

public class RegisterUsersCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUsersCommandValidator()
    {
        RuleFor(x => x.MailAddress).NotEmpty().WithMessage("Email is required!")
                                    .EmailAddress().WithMessage("Invalid Email format!")
                                    .MaximumLength(80).WithMessage("Email must be <= 80 characters");

        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is Required")
                                .MaximumLength(120).WithMessage("Address must be <= 120 character!")
                                .MinimumLength(5).WithMessage("Address must be >= 5 characters!");

        RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required!")
                                .Must(g => g == true || g == false).WithMessage("Invalid gender value format");

        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required!")
                                    .MaximumLength(50).WithMessage("First Name must be <= 50 characters!");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required!")
                                .MaximumLength(50).WithMessage("Last Name must be <= 50 characters!");

        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required!")
                                    .Matches(@"^(\+?[0-9]{1,3})?[0-9]{7,15}").WithMessage("Invalid Phone number!");

    }
}
