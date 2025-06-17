using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class SearchDossierQueryHandler : IRequestHandler<SearchDossierQuery, Result<IEnumerable<DossierSearchSanitized>>>
    {
        private readonly IDossierAglouService _dossierAglouService;
        private readonly ILogger<SearchDossierQueryHandler> _logger;
        public SearchDossierQueryHandler(IDossierAglouService dossierAglouService, ILogger<SearchDossierQueryHandler> logger)
        {
            _dossierAglouService = dossierAglouService;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<DossierSearchSanitized>>> Handle(SearchDossierQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierAglouService.SearchDossierAsync(request);
                if (result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[SearchDossier]: {0} failed execution!", nameof(SearchDossierQueryHandler));
                    return Result<IEnumerable<DossierSearchSanitized>>.Failure(new Error("The SearchDossierQueryHandler failed", "Can't handle Search dossier"));
                }
                _logger.LogInformation("[SearchDossier]: Successful operation!");
                return Result<IEnumerable<DossierSearchSanitized>>.Failure(new Error("The SearchDossierQueryHandler failed", "Can't handle Search dossier"));
            }
            catch (Exception ex)
            {
                _logger.LogError("[SearchDossier]: {0}", ex.Message);
                return Result<IEnumerable<DossierSearchSanitized>>.Failure(new Error("The SearchDossierQueryHandler failed", "Can't handle Search dossier"));
            }
        }
    }
}
