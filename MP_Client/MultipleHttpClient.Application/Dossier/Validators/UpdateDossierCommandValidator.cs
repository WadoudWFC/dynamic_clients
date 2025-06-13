using FluentValidation;
using MultipleHttpClient.Application.Dossier.Command;

namespace MultipleHttpClient.Application.Dossier.Validators
{
    public class UpdateDossierCommandValidator : AbstractValidator<UpdateDossierCommand>
    {
        public UpdateDossierCommandValidator()
        {
            RuleFor(x => x.DossierId).NotEmpty().WithMessage("Dossier ID is required");
            RuleFor(x => x.StatusId).NotEmpty().WithMessage("Status ID is required");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.AddressLocal)
                .MaximumLength(200).WithMessage("Address must be less than 200 characters");

            RuleFor(x => x.Prix)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be positive");

            RuleFor(x => x.Latitude)
                .Matches(@"^-?([1-8]?[0-9]\.\d+|90\.0+)$").When(x => !string.IsNullOrEmpty(x.Latitude))
                .WithMessage("Invalid latitude format");

            RuleFor(x => x.Longitude)
                .Matches(@"^-?((1[0-7]|[0-9])?[0-9]\.\d+|180\.0+)$").When(x => !string.IsNullOrEmpty(x.Longitude))
                .WithMessage("Invalid longitude format");

            RuleFor(x => x.Ice)
                .Length(15).When(x => !string.IsNullOrEmpty(x.Ice))
                .WithMessage("ICE must be 15 characters");
        }
    }
}
