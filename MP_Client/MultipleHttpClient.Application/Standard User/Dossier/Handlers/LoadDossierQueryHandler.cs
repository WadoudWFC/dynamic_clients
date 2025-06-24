using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class LoadDossierQueryHandler : IRequestHandler<LoadDossierQuery, Result<LoadDossierResponseSanitized>>
    {
        private readonly IDossierAglouService _dossierService;
        private readonly ILogger<LoadDossierQueryHandler> _logger;

        public LoadDossierQueryHandler(IDossierAglouService dossierService, ILogger<LoadDossierQueryHandler> logger)
        {
            _dossierService = dossierService;
            _logger = logger;
        }

        public async Task<Result<LoadDossierResponseSanitized>> Handle(LoadDossierQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierService.LoadDossierAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[LoadDossier]: Failed to load dossier {DossierId}", request.DossierId);
                    return Result<LoadDossierResponseSanitized>.Failure(new Error("LoadDossierFailed", "Unable to load dossier"));
                }

                _logger.LogInformation("[LoadDossier]: Successfully loaded dossier {DossierId}", request.DossierId);
                return Result<LoadDossierResponseSanitized>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[LoadDossier]: Exception loading dossier {DossierId}", request.DossierId);
                return Result<LoadDossierResponseSanitized>.Failure(new Error("LoadDossierFailed", ex.Message));
            }
        }
    }
}
