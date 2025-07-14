using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Command;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class InsertCommentCommandHandler : IRequestHandler<InsertCommentCommand, Result<CommentOperationResult>>
    {
        private readonly IDossierAglouService _dossierService;
        private readonly ILogger<InsertCommentCommandHandler> _logger;

        public InsertCommentCommandHandler(IDossierAglouService dossierService, ILogger<InsertCommentCommandHandler> logger)
        {
            _dossierService = dossierService;
            _logger = logger;
        }

        public async Task<Result<CommentOperationResult>> Handle(InsertCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierService.InsertCommentAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[InsertComment]: Failed to add comment to dossier {0}", request.DossierId);
                    return Result<CommentOperationResult>.Failure(new Error("InsertCommentFailed", "Unable to add comment"));
                }

                _logger.LogInformation("[InsertComment]: Successfully added comment {0} to dossier {1}",
                    result.Value.CommentId, request.DossierId);
                return Result<CommentOperationResult>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[InsertComment]: Exception adding comment to dossier {0}", request.DossierId);
                return Result<CommentOperationResult>.Failure(new Error("InsertCommentFailed", ex.Message));
            }
        }
    }
}
