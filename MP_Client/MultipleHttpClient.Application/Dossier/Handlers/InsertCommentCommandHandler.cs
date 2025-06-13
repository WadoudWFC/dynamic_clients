using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Command;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class InsertCommentCommandHandler : IRequestHandler<InsertCommentCommand, Result<CommentOperationResult>>
    {
        private readonly IDossierAglouService _dossierAglouService;
        private readonly ILogger<InsertCommentCommandHandler> _logger;
        public InsertCommentCommandHandler(IDossierAglouService dossierAglouService, ILogger<InsertCommentCommandHandler> logger)
        {
            _dossierAglouService = dossierAglouService;
            _logger = logger;
        }

        public async Task<Result<CommentOperationResult>> Handle(InsertCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierAglouService.InsertCommentAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[InsertComment]: {0} failed execution!", nameof(InsertCommentCommandHandler));
                    return Result<CommentOperationResult>.Failure(new Error("The InsertCommentCommandHandler failed", "Can't handle Insert Comment"));
                }
                _logger.LogInformation("[InsertComment]: Successful operation!");
                return Result<CommentOperationResult>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("[InsertComment]: {0}", ex.Message);
                return Result<CommentOperationResult>.Failure(new Error("The InsertCommentCommandHandler failed", ex.Message));
            }
        }
    }
}
