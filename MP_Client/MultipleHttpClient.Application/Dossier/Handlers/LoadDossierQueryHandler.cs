using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class LoadDossierQueryHandler : IRequestHandler<LoadDossierQuery, Result<LoadDossierResponseSanitized>>
    {
        private readonly IDossierAglouService _dossierAglouService;
        private readonly ILogger<LoadDossierQueryHandler> _logger;
        public LoadDossierQueryHandler(IDossierAglouService dossierAglouService, ILogger<LoadDossierQueryHandler> logger)
        {
            _dossierAglouService = dossierAglouService;
            _logger = logger;
        }

        public async Task<Result<LoadDossierResponseSanitized>> Handle(LoadDossierQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierAglouService.LoadDossierAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[LoadDossier]: {0} failed execution!", nameof(LoadDossierQueryHandler));
                    return Result<LoadDossierResponseSanitized>.Failure(new Error("The LoadDossierQueryHandler failed", "Can't handle load dossier"));
                }
                _logger.LogInformation("[LoadDossier]: Successful operation!");
                return Result<LoadDossierResponseSanitized>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("[LoadDossier]: {0}", ex.Message);
                return Result<LoadDossierResponseSanitized>.Failure(new Error("The LoadDossierQueryHandler failed", "Can't handle load dossier"));
            }
        }
    }
}
