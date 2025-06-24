using FluentValidation;
using MultipleHttpClient.Application.Dossier.Queries;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Dossier.Validators
{
    public class GetCountsQueryValidator : AbstractValidator<GetCountsQuery>
    {
        public GetCountsQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage(Constants.InvalidUser);
        }
    }
}
