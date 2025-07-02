using FluentValidation;
using MultipleHttpClient.Application.Standard_User.Dossier.Queries;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Standard_User.Dossier.Validators
{
    public class GetMyDossiersDetailedQueryValidator : AbstractValidator<GetMyDossiersDetailedQuery>
    {
        public GetMyDossiersDetailedQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage(Constants.InvalidUser)
                                    .Must(BeValidGuid).WithMessage(Constants.InvalidIdFormatError);
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role ID is required");
        }
        private bool BeValidGuid(Guid guid)
        {
            return guid != Guid.Empty && guid.ToString().Length == 36 && Guid.TryParse(guid.ToString(), out _);
        }
    }
}
