using System.Runtime.CompilerServices;
using FluentValidation;

namespace MultipleHttpClient.Application;

public class SearchHistoryQueryValidator : AbstractValidator<SearchHistoryQuery>
{
    public SearchHistoryQueryValidator()
    {
        RuleFor(x => x.DossierId).NotEmpty().WithMessage("Dossier Id is required!");

        RuleFor(x => x.Field).NotEmpty().WithMessage("Field is required!")
                                .Must(BeValidField).WithMessage("Invalid field name");

        RuleFor(x => x.Order).NotEmpty().WithMessage("Order is required!")
                                .Must(BeValidOrder).WithMessage("Invalid order name");

        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0).WithMessage("Invalid Skip value!");

        RuleFor(x => x.Take).InclusiveBetween(1, 50).WithMessage("Invalid Take value!");

    }
    private bool BeValidField(string field)
    {
        var validFields = new[] { "date_created", "dossier", "statutprecedent", "statutsuivant" };
        return validFields.Contains(field);
    }

    private bool BeValidOrder(string order)
    {
        return order == "desc" || order == "asc";
    }
}