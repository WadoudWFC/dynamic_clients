using FluentValidation;

namespace MultipleHttpClient.Application.Users.Validators;

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Password is required")
                                    .MinimumLength(8).WithMessage("Password must be >= 8 characters")
                                    .Matches("[A-Z]").WithMessage("Password requires 1 uppercase letter")
                                    .Matches("[a-z]").WithMessage("Password requires 1 lowercase letter")
                                    .Matches("[0-9]").WithMessage("Password requires 1 number")
                                    .Matches("^[a-zA-Z0-9]").WithMessage("Password requires 1 special character");
    }
}
