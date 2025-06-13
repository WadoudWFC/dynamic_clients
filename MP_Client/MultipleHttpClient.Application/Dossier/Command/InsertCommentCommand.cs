using MediatR;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Command
{
    public class InsertCommentCommand : IRequest<Result<CommentOperationResult>>
    {
        public Guid UserId { get; set; }
        public Guid DossierId { get; set; }
        public string? Content { get; set; }
    }
}
