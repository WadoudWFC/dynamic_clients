using FluentValidation;
using MultipleHttpClient.Application.Users.Commands.LoadUser;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Users.Validators
{
    public class LoadUserCommandValidator : AbstractValidator<LoadUserCommand>
    {
        public LoadUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage(Constants.InvalidUser)
                                    .Must(BeValidGuid).WithMessage(Constants.InvalidIdFormatError);
        }
        private bool BeValidGuid(Guid guid)
        {
            return guid != Guid.Empty && guid.ToString().Length == 36 && Guid.TryParse(guid.ToString(), out _);
        }
    }
}
