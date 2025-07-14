using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class GetAllCommentsQueryHandler : IRequestHandler<GetAllCommentsQuery, Result<IEnumerable<CommentSanitized>>>
    {
        private readonly IDossierAglouService _dossierService;
        private readonly ILogger<GetAllCommentsQueryHandler> _logger;

        public GetAllCommentsQueryHandler(IDossierAglouService dossierService, ILogger<GetAllCommentsQueryHandler> logger)
        {
            _dossierService = dossierService;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<CommentSanitized>>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierService.GetAllCommentsAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[GetAllComments]: Failed to get comments for dossier {0}", request.DossierId);
                    return Result<IEnumerable<CommentSanitized>>.Failure(new Error("GetCommentsFailed", "Unable to load comments"));
                }

                _logger.LogInformation("[GetAllComments]: Successfully loaded {0} comments for dossier {1}",
                    result.Value.Count(), request.DossierId);
                return Result<IEnumerable<CommentSanitized>>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GetAllComments]: Exception loading comments for dossier {0}", request.DossierId);
                return Result<IEnumerable<CommentSanitized>>.Failure(new Error("GetCommentsFailed", ex.Message));
            }
        }
    }
}
