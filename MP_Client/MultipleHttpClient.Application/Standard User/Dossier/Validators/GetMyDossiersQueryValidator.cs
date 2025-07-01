using FluentValidation;
using MultipleHttpClient.Application.Standard_User.Dossier.Queries;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetMyDossiersQueryValidator : AbstractValidator<GetMyDossiersQuery>
{
    public GetMyDossiersQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(Constants.InvalidUser)
            .Must(BeValidGuid)
            .WithMessage(Constants.InvalidIdFormatError);

        RuleFor(x => x.Take)
            .InclusiveBetween(1, 100)
            .WithMessage("Take must be between 1 and 100");

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