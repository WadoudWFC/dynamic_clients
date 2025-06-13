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
                    _logger.LogError("[GetAllComments]: {0} failed execution", nameof(GetAllCommentsQueryHandler));
                    return Result<IEnumerable<CommentSanitized>>.Failure(new Error("The GetAllCommentsQueryHandler failed", "Can't handle get comments"));
                }
                _logger.LogInformation("[GetAllComments]: Successful operation!");
                return Result<IEnumerable<CommentSanitized>>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("[GetAllComments]: {0}", ex.Message);
                return Result<IEnumerable<CommentSanitized>>.Failure(new Error("The GetAllCommentsQueryHandler failed", ex.Message));
            }
        }
    }
}
