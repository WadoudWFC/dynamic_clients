using FluentValidation;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetAllCommentQueryValidator : AbstractValidator<GetAllCommentsQuery>
{
    public GetAllCommentQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(Constants.InvalidUser)
                                    .Must(BeValidGuid).WithMessage(Constants.InvalidIdFormatError);

        RuleFor(x => x.UserId).NotEmpty().WithMessage(Constants.InvalidUser)
                                       .Must(BeValidGuid).WithMessage(Constants.InvalidIdFormatError);
    }
    private bool BeValidGuid(Guid guid)
    {
        return guid != Guid.Empty && guid.ToString().Length == 36 && Guid.TryParse(guid.ToString(), out _);
    }

}
