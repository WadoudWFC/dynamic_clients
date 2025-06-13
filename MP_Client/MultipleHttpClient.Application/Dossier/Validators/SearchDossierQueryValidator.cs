using FluentValidation;
using MultipleHttpClient.Application.Dossier.Queries;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Dossier.Validators
{
    public class SearchDossierQueryValidator : AbstractValidator<SearchDossierQuery>
    {
        private static readonly string[] AllowedFields = { "code", "status", "createddate", "partner" };
        private static readonly string[] AllowedOrders = { "asc", "desc" };

        public SearchDossierQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage(Constants.InvalidUser);
            RuleFor(x => x.RoleId).NotEmpty().When(x => x.ApplyFilter);
            RuleFor(x => x.Code).MaximumLength(20).When(x => !string.IsNullOrEmpty(x.Code))
                                .WithMessage("Code must be 20 characters or less!");
            RuleFor(x => x.Take).InclusiveBetween(1, 100).WithMessage("Take must be between 1 and 100");
            RuleFor(x => x.Skip).GreaterThanOrEqualTo(0).WithMessage("Skip must be positive");
            RuleFor(x => x.Field).Must(f => AllowedFields.Contains(f.ToLower())).WithMessage("Invalid field!");
            RuleFor(x => x.Order).Must(o => AllowedOrders.Contains(o.ToLower())).WithMessage("Invalid Order!");
            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate)
                                    .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
                                    .WithMessage("Invalid Date time!");
        }
    }
}
