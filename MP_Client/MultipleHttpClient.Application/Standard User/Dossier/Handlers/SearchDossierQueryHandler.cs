using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class SearchDossierQueryHandler : IRequestHandler<SearchDossierQuery, Result<IEnumerable<DossierSearchSanitized>>>
    {
        private readonly IDossierAglouService _dossierService;
        private readonly ILogger<SearchDossierQueryHandler> _logger;

        public SearchDossierQueryHandler(IDossierAglouService dossierService, ILogger<SearchDossierQueryHandler> logger)
        {
            _dossierService = dossierService;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<DossierSearchSanitized>>> Handle(SearchDossierQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierService.SearchDossierAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[SearchDossier]: Failed to search dossiers for user {0}", request.UserId);
                    return Result<IEnumerable<DossierSearchSanitized>>.Failure(new Error("SearchDossierFailed", "Unable to search dossiers"));
                }

                _logger.LogInformation("[SearchDossier]: Successfully searched dossiers for user {0}, found {1} results",
                    request.UserId, result.Value.Count());
                return Result<IEnumerable<DossierSearchSanitized>>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SearchDossier]: Exception searching dossiers for user {0}", request.UserId);
                return Result<IEnumerable<DossierSearchSanitized>>.Failure(new Error("SearchDossierFailed", ex.Message));
            }
        }
    }
}
