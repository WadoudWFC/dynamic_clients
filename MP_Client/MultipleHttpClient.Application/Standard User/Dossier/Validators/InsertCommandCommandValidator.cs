using FluentValidation;
using MultipleHttpClient.Application.Dossier.Command;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Dossier.Validators
{
    public class InsertCommandCommandValidator : AbstractValidator<InsertCommentCommand>
    {
        public InsertCommandCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage(Constants.InvalidUser);
            RuleFor(x => x.DossierId).NotEmpty().WithMessage(Constants.DossierFailMessage);
            RuleFor(x => x.Content).NotEmpty().WithMessage("Comment is required")
                                    .MaximumLength(100).WithMessage("Comment too long!");
        }
    }
}
