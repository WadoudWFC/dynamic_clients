using FluentValidation;
using MultipleHttpClient.Application.Dossier.Command;

namespace MultipleHttpClient.Application.Dossier.Validators
{
    public class InsertDossierCommandValidator : AbstractValidator<InsertDossierCommand>
    {
        public InsertDossierCommandValidator()
        {
            RuleFor(x => x.StatusId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.LocalAddress)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.LocalAddress));

            RuleFor(x => x.Latitude)
                .Matches(@"^-?([1-8]?[0-9]\.\d+|90\.0+)$")
                .When(x => !string.IsNullOrEmpty(x.Latitude));

            RuleFor(x => x.Longitude)
                .Matches(@"^-?((1[0-7]|[0-9])?[0-9]\.\d+|180\.0+)$")
                .When(x => !string.IsNullOrEmpty(x.Longitude));

            RuleFor(x => x.ICE)
                .Length(15).When(x => !string.IsNullOrEmpty(x.ICE));

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).When(x => x.Price.HasValue);
        }
    }
}
