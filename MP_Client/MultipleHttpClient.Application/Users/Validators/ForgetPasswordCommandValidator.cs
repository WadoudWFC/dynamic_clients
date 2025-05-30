using FluentValidation;
using MultipleHttpClient.Application.Users.Commands.ForgetPassword;

namespace MultipleHttpClient.Application.Users.Validators
{
    public class ForgetPasswordCommandValidator : AbstractValidator<ForgetPasswordCommand>
    {
        public ForgetPasswordCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required!")
                                    .EmailAddress().WithMessage("Invalid Email address!");
        }
    }
}
