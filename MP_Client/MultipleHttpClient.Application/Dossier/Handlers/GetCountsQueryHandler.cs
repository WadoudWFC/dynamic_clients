using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Queries;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class GetCountsQueryHandler : IRequestHandler<GetCountsQuery, Result<DossierCountsSanitized>>
    {
        private readonly IDossierAglouService _dossierAglouService;
        private readonly ILogger<GetCountsQueryHandler> _logger;
        public GetCountsQueryHandler(IDossierAglouService dossierAglouService, ILogger<GetCountsQueryHandler> logger)
        {
            _dossierAglouService = dossierAglouService;
            _logger = logger;
        }

        public async Task<Result<DossierCountsSanitized>> Handle(GetCountsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierAglouService.GetCountsAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[GetCounts]: {0} failed execution!", nameof(GetCountsQueryHandler));
                    return Result<DossierCountsSanitized>.Failure(new Error(Constants.DossierFail, Constants.DossierFailMessage));
                }
                _logger.LogInformation("[GetCounts]: Successful operation!");
                return Result<DossierCountsSanitized>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("[GetCounts]: {0}", ex.Message);
                return Result<DossierCountsSanitized>.Failure(new Error(Constants.DossierFail, ex.Message));
            }
        }
    }
}
