using FluentValidation;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetMyDossierIdsQueryValidator : AbstractValidator<GetMyDossierIdsQuery>
{
    public GetMyDossierIdsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(Constants.InvalidUser)
            .Must(BeValidGuid)
            .WithMessage(Constants.InvalidIdFormatError);

        RuleFor(x => x.Take)
            .InclusiveBetween(1, 500)
            .WithMessage("Take must be between 1 and 500");

        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Skip must be greater than or equal to 0");

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("Role ID is required");
    }

    private bool BeValidGuid(Guid guid)
    {
        return guid != Guid.Empty && guid.ToString().Length == 36 && Guid.TryParse(guid.ToString(), out _);
    }
}