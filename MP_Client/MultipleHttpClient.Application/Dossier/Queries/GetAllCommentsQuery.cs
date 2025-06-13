using MediatR;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetAllCommentsQuery : IRequest<Result<IEnumerable<CommentSanitized>>>
{
    public Guid DossierId { get; set; }
    public Guid UserId { get; set; }
}
